namespace Prateek.Runtime.AppContentFramework.ContentAccess.Daemons
{
    using Prateek.Runtime.AppContentFramework.ContentAccess.Interfaces;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class ContentAccessDaemonOverseer<TDaemon, TServant>
        : DaemonOverseer<TDaemon, TServant>
        , IEarlyUpdateTickable
        , IContentAccessorOwner
        where TDaemon : ContentAccessDaemonOverseer<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
    {
        #region Class Methods
        protected override void OnServantRegistered(TServant servant)
        {
            base.OnServantRegistered(servant);

            servant.SetupContentAccess(this.Get<ContentAccessor>());
        }
        #endregion

        #region IContentAccessorOwner Members
        public virtual void DefineReceptionActions(ICommandReceiver receiver) { }

        public virtual void SetupContentAccess(ContentAccessor contentAccessor) { }
        #endregion

        #region IEarlyUpdateTickable Members
        public virtual void EarlyUpdate()
        {
            this.Get<ICommandReceiver>().ProcessReceivedCommands();
        }

        public int Priority(IPriority<IEarlyUpdateTickable> type)
        {
            return DefaultPriority;
        }
        #endregion
    }
}
