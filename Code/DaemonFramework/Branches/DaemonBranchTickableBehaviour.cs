namespace Prateek.DaemonFramework.Code.Servants
{
    using Prateek.DaemonFramework.Code.Interfaces;
    using Prateek.TickableFramework.Code;
    using Prateek.TickableFramework.Code.Enums;
    using Prateek.TickableFramework.Code.Interfaces;

    public abstract class ServantTickableBehaviour<TDaemon, TServant>
        : ServantBehaviour<TDaemon, TServant>, ITickable
        where TDaemon : Daemon<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Class Methods
        public override void Startup()
        {
            base.Startup();

            TickableRegistry.Register(this);
        }

        public override void Shutdown()
        {
            TickableRegistry.Unregister(this);

            base.Shutdown();
        }
        #endregion

        #region ITickable Members
        public virtual TickableSetup TickableSetup
        {
            get { return TickableSetup.Nothing; }
        }

        public virtual void InitializeTickable() { }

        public virtual void TickFixed(TickableFrame tickableFrame, float seconds) { }

        public virtual void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds) { }

        public virtual void TickLate(TickableFrame tickableFrame, float seconds) { }

        public virtual void ApplicationIsQuitting() { }

        public virtual void ApplicationChangeFocus(bool appStatus) { }

        public virtual void ApplicationChangePause(bool appStatus) { }

        public virtual void DrawGUI() { }
        #endregion
    }
}
