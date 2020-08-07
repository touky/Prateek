namespace Prateek.Runtime.DaemonFramework.Servants
{
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.TickableFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class TickableServant<TDaemon, TServant>
        : Servant<TDaemon, TServant>
        , ITickable
        where TDaemon : DaemonOverseer<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Class Methods
        public override void Startup()
        {
            base.Startup();

            this.RegisterTickable();
        }

        public override void Shutdown()
        {
            this.UnregisterTickable();

            base.Shutdown();
        }
        #endregion
    }
}
