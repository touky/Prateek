namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.Core.HierarchicalTree;

    public abstract class ContentAccessDaemon<TDaemon>
        : ReceiverDaemon<TDaemon>
        where TDaemon : ReceiverDaemon<TDaemon>
    {
        #region Properties
        public abstract string[] ResourceKeywords { get; }

        public virtual HierarchicalTreeSettingsData Settings { get { return null; } }
        #endregion

        #region Register/Unregister
        protected override void OnAwake()
        {
            base.OnAwake();

            CommandReceiver.Send(GetAccessRequest());
        }
        #endregion

        #region Class Methods
        public override void DefineCommandReceiverActions()
        {
            CommandReceiver.SetActionFor<ContentAccessChangedResponse>(OnContentAccessChangedResponse);
        }

        private void OnContentAccessChangedResponse(ContentAccessChangedResponse response)
        {
            ContentAccessChanged(response);
        }

        internal ContentAccessRequest GetAccessRequest()
        {
            var request = CreateContentAccessRequest();
            request.Init(ResourceKeywords, Settings);
            return request;
        }

        protected abstract ContentAccessRequest CreateContentAccessRequest();
        protected abstract void ContentAccessChanged(ContentAccessChangedResponse response);
        #endregion
    }
}
