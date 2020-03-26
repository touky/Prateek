namespace Assets.Prateek.ToConvert.StateMachines.Interfaces
{
    using System;

    public interface ISequentialStateMachineOwner<TState> : ISimpleStateMachineOwner<TState, SequentialTriggerType>
        where TState : struct, IConvertible { }
}
