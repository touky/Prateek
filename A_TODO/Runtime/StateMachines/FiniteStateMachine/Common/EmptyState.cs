namespace Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine.Common
{
    public class EmptyState<TTrigger> : FiniteState<TTrigger>
    {
        #region Class Methods
        protected override void Begin() { }
        protected override void End() { }
        public override void Execute() { }
        #endregion
    }
}
