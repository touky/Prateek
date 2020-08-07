namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    using Prateek.Runtime.TickableFramework.Interfaces;

    internal class PostLateUpdateTickableGroup : TickableGroup<IPostLateUpdateTickable>
    {
        #region Properties
        public override bool InjectAtTheEnd
        {
            get { return false; }
        }
        #endregion

        #region Class Methods
        public override void Tick(IPostLateUpdateTickable tickable)
        {
            tickable.PostLateUpdate();
        }
        #endregion
    }
}
