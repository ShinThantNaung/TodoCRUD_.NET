using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System;
using Todo.Model;
using Todo.Controllers;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TodoDb>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();


app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/", () => "HelloWorld");

app.MapTodoEndpoints();

app.Run();