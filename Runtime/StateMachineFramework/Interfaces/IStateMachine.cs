namespace Prateek.Runtime.StateMachineFramework.Interfaces
{
    using Prateek.Runtime.StateMachineFramework.EnumStateMachines;

    public interface IStateMachine<TState, TTrigger>
        : DelegateStateMachine.IMachine
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
    
    public interface IEnumComparer<TEnum>
    {
        #region Class Methods
        /// <summary>
        ///     This Compare method is needed because C# generic are a pain to make work without GC
        /// </summary>
        /// <param name="enum0"></param>
        /// <param name="enum1"></param>
        /// <returns></returns>
        bool Compare(TEnum enum0, TEnum enum1);
        #endregion
    }

    public interface IEnumComparer<TEnum0, TEnum1>
    {
        #region Class Methods
        /// <summary>
        ///     This Compare method is needed because C# generic are a pain to make work without GC
        /// </summary>
        /// <param name="enum0"></param>
        /// <param name="enum1"></param>
        /// <returns></returns>
        bool Compare(TEnum0 enum0, TEnum0 enum1);

        /// <summary>
        ///     This Compare method is needed because C# generic are a pain to make work without GC
        /// </summary>
        /// <param name="enum0"></param>
        /// <param name="enum1"></param>
        /// <returns></returns>
        bool Compare(TEnum1 enum0, TEnum1 enum1);
        #endregion
    }
}
