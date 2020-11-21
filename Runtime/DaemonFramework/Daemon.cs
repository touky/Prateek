namespace Prateek.Runtime.DaemonFramework
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Singleton;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;

    /// <summary>
    /// Daemons are <seealso cref="SingletonBehaviour{TInstance}"/> that acts as backend independent system
    /// The term "Daemon" is derived from the unix one, but can also be refered as "Service" in the microsoft ecosystem
    /// - Daemon embark a <seealso cref="GadgetPouch"/> and use <seealso cref="AutoRegisterExtensions.AutoRegister"/> and the <seealso cref="ITickable"/> interface
    /// </summary>
    /// <typeparam name="TDaemon"></typeparam>
    public abstract class Daemon<TDaemon>
        : SingletonBehaviour<TDaemon>
        , ITickable
        , IGadgetOwner
        where TDaemon : Daemon<TDaemon>
    {
        #region Fields
        private GadgetPouch gadgetPouch = new GadgetPouch();
        #endregion

        #region Unity Methods
        protected virtual void OnDestroy()
        {
            this.AutoUnregister();
        }
        #endregion

        #region Register/Unregister
        protected override void OnAwake()
        {
            this.AutoRegister();
        }
        #endregion

        #region IGadgetOwner Members
        public GadgetPouch GadgetPouch
        {
            get { return gadgetPouch; }
        }

        public string Name
        {
            get { return name; }
        }
        #endregion

        #region ITickable Members
        public int DefaultPriority
        {
            get { return 0; }
        }
        #endregion
    }
}
