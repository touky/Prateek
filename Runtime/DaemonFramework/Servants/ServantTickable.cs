namespace Prateek.Runtime.DaemonFramework.Servants
{
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.TickableFramework;
    using Prateek.Runtime.TickableFramework.Enums;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class ServantTickable<TDaemon, TServant>
        : Servant<TDaemon, TServant>, ITickable
        where TDaemon : DaemonOverseer<TDaemon, TServant>
        where TServant : class, IServant
    {
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
