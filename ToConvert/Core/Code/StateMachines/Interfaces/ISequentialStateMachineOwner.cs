namespace Mayfair.Core.Code.StateMachines.Interfaces
{
    using System;

    public interface ISequentialStateMachineOwner<TState> : ISimpleStateMachineOwner<TState, SequentialTriggerType>
        where TState : struct, IConvertible { }
}
