namespace W12CSheets.Client.Helpers;

/// <summary>
/// State machine helper
/// </summary>
public class StateMachine<TState, TTrigger> where TState : struct where TTrigger : struct
{
    private TState _currentState;
    private readonly Dictionary<TState, Dictionary<TTrigger, TState>> _transitions = new();
    private readonly Dictionary<TState, Action?> _onEnter = new();
    private readonly Dictionary<TState, Action?> _onExit = new();

    public StateMachine(TState initialState)
    {
        _currentState = initialState;
    }

    /// <summary>
    /// Configure state transition
    /// </summary>
    public void Configure(TState fromState, TTrigger trigger, TState toState)
    {
        if (!_transitions.ContainsKey(fromState))
        {
            _transitions[fromState] = new Dictionary<TTrigger, TState>();
        }
        
        _transitions[fromState][trigger] = toState;
    }

    /// <summary>
    /// Set on enter callback
    /// </summary>
    public void OnEnter(TState state, Action action)
    {
        _onEnter[state] = action;
    }

    /// <summary>
    /// Set on exit callback
    /// </summary>
    public void OnExit(TState state, Action action)
    {
        _onExit[state] = action;
    }

    /// <summary>
    /// Fire trigger
    /// </summary>
    public bool Fire(TTrigger trigger)
    {
        if (_transitions.TryGetValue(_currentState, out var stateTriggers) &&
            stateTriggers.TryGetValue(trigger, out var nextState))
        {
            if (_onExit.TryGetValue(_currentState, out var exitAction))
            {
                exitAction?.Invoke();
            }

            _currentState = nextState;

            if (_onEnter.TryGetValue(_currentState, out var enterAction))
            {
                enterAction?.Invoke();
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Get current state
    /// </summary>
    public TState CurrentState => _currentState;

    /// <summary>
    /// Check if trigger is valid for current state
    /// </summary>
    public bool CanFire(TTrigger trigger)
    {
        return _transitions.TryGetValue(_currentState, out var triggers) &&
               triggers.ContainsKey(trigger);
    }
}
