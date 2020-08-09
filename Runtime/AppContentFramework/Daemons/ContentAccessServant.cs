namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.DaemonFramework.Servants;

    public abstract class ContentAccessServant<TDaemon, TServant>
        : Servant<TDaemon, TServant>
        where TDaemon : ReceiverDaemonOverseer<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
    {
        #region Fields
        private ContentAccessRequest accessRequest;
        #endregion

        #region Properties
        public abstract string[] ResourceKeywords { get; }

        public virtual HierarchicalTreeSettingsData Settings { get { return null; } }
        #endregion

        #region Class Methods
        internal ContentAccessRequest GetAccessRequest()
        {
            accessRequest = CreateContentAccessRequest();
            accessRequest.Init(ResourceKeywords, Settings);
            return accessRequest;
        }

        internal void ContentAccessChanged(ContentAccessChangedResponse response)
        {
            if (response.request != accessRequest)
            {
                return;
            }

            ContentAccessChanged(response);
        }

        protected abstract ContentAccessRequest CreateContentAccessRequest();
        protected abstract void OnContentAccessChangedResponse(ContentAccessChangedResponse response);
        #endregion
    }
}
