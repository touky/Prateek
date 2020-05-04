namespace Mayfair.Core.Code.StateMachines.Interfaces
{
    using System;

    public interface ISimpleStepMachineOwner<TState>
        : ISimpleStateMachineOwner<TState, SimpleStepTrigger>
        where TState : struct, IConvertible { }
}
