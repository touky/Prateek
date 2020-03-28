namespace Mayfair.Core.Code.StateMachines.FSM.Common
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
