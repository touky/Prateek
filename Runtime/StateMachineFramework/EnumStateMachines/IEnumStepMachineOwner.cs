namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;

    public interface IEnumStepMachineOwner<TState>
        : IEnumStateMachineOwner<TState>
        where TState : struct, IConvertible { }
}
