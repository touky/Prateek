namespace Prateek.Runtime.DaemonFramework.Servants
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;

    /// <summary>
    /// This servant implements the <seealso cref="ITickable"/> interface for all your tickable needs
    /// </summary>
    /// <typeparam name="TDaemon">The Deamon class type class is registering to</typeparam>
    /// <typeparam name="TServant">The Servant class type inheriting from this class</typeparam>
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
