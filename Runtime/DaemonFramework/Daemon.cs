namespace Prateek.Runtime.DaemonFramework
{
    using System;
    using Prateek.Runtime.Core.Singleton;
    using Prateek.Runtime.TickableFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class Daemon<TDaemon>
        : SingletonBehaviour<TDaemon>
        , ITickable
        where TDaemon : Daemon<TDaemon>
    {
        #region Properties
        public virtual int Priority
        {
            get { return 0; }
        }
        #endregion

        protected override void OnAwake()
        {
            this.RegisterTickable();
        }

        protected virtual void OnDestroy()
        {
            this.UnregisterTickable();
        }
    }
}
