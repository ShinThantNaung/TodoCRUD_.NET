using Todo.Controllers;
using Todo.Model;
namespace TodoApi.Test;

public class TodoEncapsulationTest
{
    [Fact]

    public void CreateNewTodo()
    {
        var Todo = new TodoItem("Learn dotnet");
        var name = Todo.Name;

        Assert.Equal("Learn dotnet", name);
    }
    [Fact]
    public void ThrowExceptionOnEmptyName()
    {
        var Exception = Assert.Throws<Exception>(() =>
        {
            new TodoItem(" ");
        });

        Assert.Equal("The name of the todo cannot be empty", Exception.Message);
    }
    [Fact]
    public void MarkCompleteTest()
    {
        var Todo = new TodoItem("Learn dotnet");
        Todo.MarkAsComplete();
        Assert.True(Todo.IsComplete);
    }
    [Fact]
    public void ChangeTodoName()
    {
        var Todo = new TodoItem("Learn dotnet");
        Todo.UpdateName("New dotnet");

        Assert.Equal("New dotnet", Todo.Name);
    }
    [Fact]
    public void ThrowExceptionOnEmptyNameUpdate()
    {
        var Todo = new TodoItem("Learn dotnet");
        var Exception = Assert.Throws<Exception>(() =>
        {
            Todo.UpdateName(" ");
        });
        Assert.Equal("The name of the todo cannot be empty", Exception.Message);
    }
}
