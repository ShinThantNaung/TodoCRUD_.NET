using Todo.Model;
namespace Todo.DTO
{
    public record CreateTodo(String name, Boolean isComplete, String? secret);

    public record UpdateTodo(int id, String name, Boolean isComplete, String? secret);

    public record ApiResponse<T>(
        bool Success,
        int HttpStatus,
        string Message,
        T? Data = default
    ) : IResult
    {
        public static ApiResponse<T> Ok(T data, string message = "Success") =>
            new(true, StatusCodes.Status200OK, message, data);

        public static ApiResponse<T> Created(T data, string message = "Created") =>
            new(true, StatusCodes.Status201Created, message, data);

        public static ApiResponse<T> NotFound(string message = "Resource not found") =>
            new(false, StatusCodes.Status404NotFound, message, default);

        public static ApiResponse<T> BadRequest(string message = "Bad Request") =>
            new(false, StatusCodes.Status400BadRequest, message, default);

        

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = HttpStatus;
            await httpContext.Response.WriteAsJsonAsync(this);
        }
    }

    public record UpdateNameRequest(string NewName);

    public record CreateNewTodoRequest(TodoItem todo);

    public record UpdateTodoRequest(TodoItem todo, TodoItem InputTodo);
}
