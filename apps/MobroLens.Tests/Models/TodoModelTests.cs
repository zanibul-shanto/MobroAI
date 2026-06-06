using MobroLens.Models;

namespace MobroLens.Tests.Models;

public class TodoModelTests
{
    [Fact]
    public void Todo_DefaultValues_AreCorrect()
    {
        var todo = new Todo();

        Assert.Equal(0, todo.Id);
        Assert.Null(todo.Name);
        Assert.False(todo.IsComplete);
    }

    [Fact]
    public void Todo_CanBeMarkedComplete()
    {
        var todo = new Todo
        {
            Id = 1,
            Name = "Test Todo",
            IsComplete = true
        };

        Assert.Equal(1, todo.Id);
        Assert.Equal("Test Todo", todo.Name);
        Assert.True(todo.IsComplete);
    }
}
