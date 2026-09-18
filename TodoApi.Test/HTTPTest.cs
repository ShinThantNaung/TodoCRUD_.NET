using System;
using System.Collections.Generic;
using System.Text;
using Todo.Controllers;
using Todo.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using System.Net.Http.Json;

namespace TodoApi.Test
{
    public class HTTPTest: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public HTTPTest(WebApplicationFactory<Program> Factory)
        {
            _client = Factory.CreateClient();
        }
        [Fact]
        public async Task GetTodos_ReturnSuccess()
        {
            var response = await _client.GetAsync("/todos");
            var todo = response.Content.ReadFromJsonAsync<TodoItem>();
            Assert.NotNull(todo);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetTodosById_ReturnSuccess()
        {
            var response = await _client.GetAsync("/todos/1");

            var todo = response.Content.ReadFromJsonAsync<TodoItem>();

            Assert.NotNull(todo);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetTodosById_ReturnNotFound()
        {
            var response = await _client.GetAsync("/todos/1000");
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateTodo_ReturnSuccess()
        {
            var payload = new
            {
                Name = "Test",
                IsCompleted = false
            };
            HttpResponseMessage response = await _client.PostAsJsonAsync("/todos/", payload);

            var todo = await response.Content.ReadFromJsonAsync<TodoItem>();

            Assert.NotNull(todo);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(payload.Name, todo.Name);
            Assert.Equal(payload.IsCompleted, todo.IsComplete);
        }
        [Fact]
        public async Task CreateTodo_ReturnBadRequest()
        {
            var payload = new
            {
                Name = "Test",
                IsCompleted = false
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("todos/", payload);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateTodo_SuccessRequest()
        {
            var payload = new
            {
                Name = "Test",
                IsCompleted = false
            };

            HttpResponseMessage postResponse = await _client.PostAsJsonAsync("todos/", payload);

            var todo = await postResponse.Content.ReadFromJsonAsync<TodoItem>();

            Assert.NotNull(todo);

            int id = todo.Id;

            var newPayload = new
            {
                Name = "New Test",
                IsCompleted = false
            };
            HttpResponseMessage response = await _client.PostAsJsonAsync($"todos/{id}", payload);

            var newTodo = await response.Content.ReadFromJsonAsync<TodoItem>();

            Assert.NotNull(newTodo);
            Assert.Equal(newTodo.Name, newPayload.Name);
            Assert.Equal(newTodo.IsComplete, newPayload.IsCompleted);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateName_SuccessRequest()
        {
            var payload = new
            {
                Name = "Test",
                IsCompleted = false
            };

            HttpResponseMessage postResponse = await _client.PostAsJsonAsync("todos/", payload);

            var todo = await postResponse.Content.ReadFromJsonAsync<TodoItem>();

            Assert.NotNull(todo);

            int id = todo.Id;

            var newPayload = new
            {
                Name = "New Test"
            };

            HttpResponseMessage response = await _client.PatchAsJsonAsync($"todos/{id}/name", newPayload);
            var newTodo = await response.Content.ReadFromJsonAsync<TodoItem>();
            Assert.NotNull(newTodo);
            Assert.Equal(newTodo.Name, newPayload.Name);
            Assert.True(response.IsSuccessStatusCode);
        }
        [Fact]
        public async Task MarkComplete_SuccessRequest()
        {
            var payload = new
            {
                Name = "Test",
                IsCompleted = false
            };

            HttpResponseMessage postResponse = await _client.PostAsJsonAsync("todos/", payload);

            var todo = await postResponse.Content.ReadFromJsonAsync<TodoItem>();

            Assert.NotNull(todo);

            int id = todo.Id;

            HttpResponseMessage response = await _client.PatchAsync($"todos/{id}/status",null);
            var newTodo = await response.Content.ReadFromJsonAsync<TodoItem>();
            Assert.NotNull(newTodo);
            Assert.True(newTodo.IsComplete);
            Assert.True(response.IsSuccessStatusCode);
        }
        [Fact]
        public async Task DeleteTodo_SuccessRequest()
        {
            var payload = new
            {
                Name = "Test",
                IsCompleted = false
            };

            HttpResponseMessage postResponse = await _client.PostAsJsonAsync("todos/", payload);

            var todo = await postResponse.Content.ReadFromJsonAsync<TodoItem>();

            Assert.NotNull(todo);

            int id = todo.Id;

            HttpResponseMessage response = await _client.DeleteAsync($"todos/{id}");

            HttpResponseMessage getResponse = await _client.GetAsync($"todos/{id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
