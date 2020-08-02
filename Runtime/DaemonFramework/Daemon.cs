namespace Prateek.Runtime.DaemonFramework
{
    using Prateek.Runtime.Core.Singleton;
    using Prateek.Runtime.TickableFramework.Enums;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class Daemon<TDaemon>
        : SingletonBehaviour<TDaemon>, ITickable
        where TDaemon : Daemon<TDaemon>
    {
        #region ITickable Members
        public virtual int Priority
        {
            get { return 0; }
        }

        public abstract TickableSetup TickableSetup { get; }

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
