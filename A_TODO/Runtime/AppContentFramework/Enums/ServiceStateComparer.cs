namespace Prateek.A_TODO.Runtime.AppContentFramework.Enums
{
    using Prateek.Runtime.StateMachineFramework.EnumStateMachines;

    public class ServiceStateComparer : EnumStepTriggerComparer<ServiceState>
    {
        public override bool Compare(ServiceState state0, ServiceState state1)
        {
            return state0 == state1;
        }
    }
}
