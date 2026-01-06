using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class StateMachineTests
{
    enum State { Start, Running, Stopped }
    enum Trigger { Begin, Stop, Reset }

    [Fact]
    public void Fire_ShouldTransitionState()
    {
        var sm = new StateMachine<State, Trigger>(State.Start);
        sm.Configure(State.Start, Trigger.Begin, State.Running);
        
        var result = sm.Fire(Trigger.Begin);
        
        Assert.True(result);
        Assert.Equal(State.Running, sm.CurrentState);
    }

    [Fact]
    public void CanFire_ShouldReturnTrue_WhenValid()
    {
        var sm = new StateMachine<State, Trigger>(State.Start);
        sm.Configure(State.Start, Trigger.Begin, State.Running);
        
        Assert.True(sm.CanFire(Trigger.Begin));
    }
}
