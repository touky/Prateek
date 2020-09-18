namespace Prateek.Runtime.StateMachineFramework.Interfaces
{
    public interface IStateMachine<TState, TTrigger>
    {
        #region Properties
        TState ActiveState { get; }
        TState IncomingState { get; }
        bool IsActive { get; }
        #endregion

        #region Class Methods
        void Reboot();
        void Step();
        void Trigger(TTrigger trigger);
        #endregion
    }

    public interface IEnumComparer<TState, TTrigger>
    {
        #region Class Methods
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
        #endregion
    }
}
