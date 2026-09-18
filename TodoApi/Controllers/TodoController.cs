using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Todo.Model;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Todo.DTO;

namespace Todo.Controllers
{
    public static class TodoController
    {
        public static void MapTodoEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/todos");

            group.MapGet("/", async (TodoDb db) =>
            {
                var todos = await db.Todos.ToListAsync();
                if(todos.Count == 0)
                {
                    return ApiResponse<List<TodoItem>>.NotFound();
                }
                return ApiResponse<List<TodoItem>>.Ok(todos);
            });

            group.MapGet("/{id}", async (int id, TodoDb db) =>
            {
                var todo = await db.Todos.FindAsync(id);
                if (todo == null)
                {
                    return ApiResponse<TodoItem>.NotFound();
                }
                return ApiResponse<TodoItem>.Ok(todo);
            });

            group.MapPost("/", async (CreateNewTodoRequest request, TodoDb db) =>
            {
                if(request.todo is null)
                {
                    return ApiResponse<TodoItem>.BadRequest();
                }
                db.Todos.Add(request.todo);
                await db.SaveChangesAsync();
                return ApiResponse<TodoItem>.Created(request.todo);
            });

            group.MapPut("/{id}", async (int id, TodoDb db, UpdateTodoRequest request) =>
            {
                var todo = await db.Todos.FindAsync(id);
                if(todo is null)
                {
                    return ApiResponse<TodoItem>.NotFound();
                }
                if(request.InputTodo is null)
                {
                    return ApiResponse<TodoItem>.BadRequest();
                }
                await db.SaveChangesAsync();
                return ApiResponse<TodoItem>.Ok(request.InputTodo);
            });

            group.MapPatch("/{id:int}/status", async (int id, TodoDb db) =>
            {
                var todo = await db.Todos.FindAsync(id);
                if(todo is null)
                {
                    return ApiResponse<TodoItem>.NotFound();
                }
                todo.MarkAsComplete();
                 await db.SaveChangesAsync();
                return ApiResponse<TodoItem>.Ok(todo);
            });

            group.MapPatch("/{id:int}/name", async (int id, TodoDb db, UpdateNameRequest request) =>
            { 
                if (string.IsNullOrWhiteSpace(request.NewName))
                {
                    return ApiResponse<TodoItem>.BadRequest("The new name cannot be empty");
                }
                var todo = await db.Todos.FindAsync(id);
                if (todo is null)
                {
                    return ApiResponse<TodoItem>.NotFound();
                }
                todo.UpdateName(request.NewName);
                await db.SaveChangesAsync();
                return ApiResponse<TodoItem>.Ok(todo);
            });

            group.MapDelete("/{id}", async (int id,TodoDb db) =>
            { 
                if (await db.Todos.FindAsync(id) is TodoItem todo)
                {
                    db.Todos.Remove(todo);
                    await db.SaveChangesAsync();
                    return ApiResponse<TodoItem>.Ok(todo);
                }
                return ApiResponse<TodoItem>.NotFound();

            });
        }
    }
}
