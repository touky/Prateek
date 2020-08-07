namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    using Prateek.Runtime.TickableFramework.Interfaces;

    internal class PreLateUpdateTickableGroup : TickableGroup<IPreLateUpdateTickable>
    {
        #region Properties
        public override bool InjectAtTheEnd
        {
            get { return true; }
        }
        #endregion

        #region Class Methods
        public override void Tick(IPreLateUpdateTickable tickable)
        {
            tickable.PreLateUpdate();
        }
        #endregion
    }
}
