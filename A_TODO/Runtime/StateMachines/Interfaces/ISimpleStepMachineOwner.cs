namespace Prateek.A_TODO.Runtime.StateMachines.Interfaces
{
    using System;
    using Prateek.A_TODO.Runtime.StateMachines.SimpleStateMachine;

    public interface ISimpleStepMachineOwner<TState>
        : ISimpleStateMachineOwner<TState, SimpleStepTrigger>
        where TState : struct, IConvertible { }
}
