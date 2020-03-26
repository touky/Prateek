namespace Assets.Prateek.ToConvert.StateMachines.Interfaces
{
    using System;

    public interface ISimpleStateMachineOwner<TState, TTrigger> : IStateMachineOwner
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible
    {
        #region Class Methods
        /// <summary>
        ///     Callback to inform the owner of a state change.
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="nextState"></param>
        void StateChange(TState previousState, TState nextState);

        /// <summary>
        ///     Callback to inform the owner that this state is executing
        /// </summary>
        /// <param name="state"></param>
        void ExecuteState(TState state);

        /// <summary>
        ///     This Compare method is needed because C# generic are a pain to make work without GC
        /// </summary>
        /// <param name="state0"></param>
        /// <param name="state1"></param>
        /// <returns></returns>
        bool Compare(TState state0, TState state1);

        /// <summary>
        ///     This Compare method is needed because C# generic are a pain to make work without GC
        /// </summary>
        /// <param name="trigger0"></param>
        /// <param name="trigger1"></param>
        /// <returns></returns>
        bool Compare(TTrigger trigger0, TTrigger trigger1);

        /// <summary>
        ///     Called when a trigger is received
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="hasTriggered">true if a transition has been triggered</param>
        void OnTrigger(TTrigger trigger, bool hasTriggered);
        #endregion
    }
}
