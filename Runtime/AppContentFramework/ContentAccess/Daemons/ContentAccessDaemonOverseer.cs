namespace Prateek.Runtime.AppContentFramework.ContentAccess.Daemons
{
    using Prateek.Runtime.AppContentFramework.ContentAccess.Gadgets;
    using Prateek.Runtime.AppContentFramework.ContentAccess.Interfaces;
    using Prateek.Runtime.CommandFramework.Gadgets;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class ContentAccessDaemonOverseer<TDaemon, TServant>
        : DaemonOverseer<TDaemon, TServant>
        , IEarlyUpdateTickable
        , ContentAccess.IAccessorOwner
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
        public ContentAccess.IAccessor ContentAccessor { get; private set; }
        public CommandTools.IReceiver Receiver { get; private set; }

        public virtual void DefineReceptionActions(CommandTools.IReceiver receiver) { }

        public virtual void SetupContentAccess(ContentAccess.IAccessor contentAccessor) { }
        #endregion

        #region IEarlyUpdateTickable Members
        public virtual void EarlyUpdate()
        {
            this.Get<CommandTools.IReceiver>().ProcessReceivedCommands();
        }

        public int Priority(IPriority<IEarlyUpdateTickable> type)
        {
            return DefaultPriority;
        }
        #endregion
    }
}
