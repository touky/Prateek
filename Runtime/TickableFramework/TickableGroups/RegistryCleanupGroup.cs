namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    /// <summary>
    ///     Internal class to Clear & Flush (un)registering tickable
    /// </summary>
    internal class RegistryCleanupGroup : TickableGroup
    {
        #region Properties
        public override bool InjectAtTheEnd
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        public RegistryCleanupGroup()
        {
            name = "Initialization";
        }
        #endregion

        #region Register/Unregister
        public override void Register(AliveTickable tickable) { }
        #endregion

        #region Class Methods
        public override void Tick()
        {
            TickableRegistry.ClearDeadTickables();
            TickableRegistry.FlushPendingRegistry();
        }

        public override void Unregister(AliveTickable tickable) { }
        #endregion
    }
}
