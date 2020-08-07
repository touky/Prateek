namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    using Prateek.Runtime.TickableFramework.Interfaces;

    internal class EarlyUpdateTickableGroup : TickableGroup<IEarlyUpdateTickable>
    {
        #region Properties
        public override bool InjectAtTheEnd
        {
            get { return true; }
        }
        #endregion

        #region Class Methods
        public override void Tick(IEarlyUpdateTickable tickable)
        {
            tickable.EarlyUpdate();
        }
        #endregion
    }
}
