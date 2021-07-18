namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    public class EnumStepTriggerComparer
        : IEnumComparer<EnumStepTrigger>
    {
        public EnumStepTriggerComparer()
        {
        }

        #region IEnumComparer<TState,SimpleStepTrigger> Members
        public bool Compare(EnumStepTrigger enum0, EnumStepTrigger enum1)
        {
            return enum0 == enum1;
        }
        #endregion
    }
}
