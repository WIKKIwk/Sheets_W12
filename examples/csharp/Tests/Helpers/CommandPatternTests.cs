using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class CommandPatternTests
{
    private class TestCommand : ICommand
    {
        public bool Executed { get; private set; }
        public bool Undone { get; private set; }

        public void Execute() => Executed = true;
        public void Undo() => Undone = true;
    }

    [Fact]
    public void Execute_ShouldExecuteCommand()
    {
        var invoker = new CommandInvoker();
        var command = new TestCommand();
        
        invoker.Execute(command);
        
        Assert.True(command.Executed);
    }

    [Fact]
    public void Undo_ShouldUndoCommand()
    {
        var invoker = new CommandInvoker();
        var command = new TestCommand();
        
        invoker.Execute(command);
        invoker.Undo();
        
        Assert.True(command.Undone);
    }

    [Fact]
    public void CanUndo_ShouldReturnTrue_AfterExecute()
    {
        var invoker = new CommandInvoker();
        var command = new TestCommand();
        
        invoker.Execute(command);
        
        Assert.True(invoker.CanUndo);
    }
}
