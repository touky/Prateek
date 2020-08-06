namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    public interface IEnumStateMachineOwner<TState>
        : IStateMachineOwner
        where TState : struct, IConvertible
    {
        #region Class Methods
        /// <summary>
        ///     Callback to inform the owner of a state change.
        /// </summary>
        /// <param name="endingState"></param>
        /// <param name="beginningState"></param>
        void ChangingState(TState endingState, TState beginningState);

        /// <summary>
        ///     Callback to inform the owner that this state is executing
        /// </summary>
        /// <param name="state"></param>
        void ExecutingState(TState state);
        #endregion
    }
}
