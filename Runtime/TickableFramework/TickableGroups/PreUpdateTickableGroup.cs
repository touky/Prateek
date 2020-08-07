namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    using Prateek.Runtime.TickableFramework.Interfaces;

    internal class PreUpdateTickableGroup : TickableGroup<IPreUpdateTickable>
    {
        #region Properties
        public override bool InjectAtTheEnd
        {
            get { return true; }
        }
        #endregion

        #region Class Methods
        public override void Tick(IPreUpdateTickable tickable)
        {
            tickable.PreUpdate();
        }
        #endregion
    }
}
