namespace Prateek.Runtime.DaemonFramework.Servants
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.DaemonFramework.Interfaces;
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

            this.AutoRegister();
        }

        public override void Shutdown()
        {
            this.AutoUnregister();

            base.Shutdown();
        }
        #endregion
    }
}
