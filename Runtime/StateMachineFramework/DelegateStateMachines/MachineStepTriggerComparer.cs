namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    public class MachineStepTriggerComparer
        : IEnumComparer<MachineStepTrigger>
    {
        public MachineStepTriggerComparer()
        {
        }

        #region IEnumComparer<TState,SimpleStepTrigger> Members
        public bool Compare(MachineStepTrigger enum0, MachineStepTrigger enum1)
        {
            return enum0 == enum1;
        }
        #endregion
    }
}
