namespace Mayfair.Core.Code.StateMachines.Interfaces
{
    /// <summary>
    ///     A simple state machine interface
    ///     Usage:
    ///     Declare a StateMachine with your state type.
    ///     Add the IStateMachineOwner interface and Compare callbacks to your class
    ///     To update the state machine and go to the next state, just call Advance()
    /// </summary>
    /// <typeparam name="TState">The state type used for the state machine</typeparam>
    /// <typeparam name="TTrigger">The trigger type used to trigger a reaction of the machine</typeparam>
    public interface IStateMachine<TState, TTrigger>
    {
        TState CurrentState { get; }
        TState NextState { get; }
        bool IsRunning { get; }
        void Restart();
        void Advance();
        void Trigger(TTrigger trigger);
    }
}