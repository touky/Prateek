namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    public abstract class EnumStepTriggerComparer<TState>
        : IEnumComparer<TState, EnumStepTrigger>
        where TState : struct, IConvertible
    {
        #region IEnumComparer<TState,SimpleStepTrigger> Members
        public abstract bool Compare(TState state0, TState state1);

        public bool Compare(EnumStepTrigger trigger0, EnumStepTrigger trigger1)
        {
            return trigger0 == trigger1;
        }
        #endregion
    }
}
