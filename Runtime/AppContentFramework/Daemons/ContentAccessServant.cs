namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.DaemonFramework.Servants;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class ContentAccess
        : IGadget
    {
        private ContentAccessRequest accessRequest;

        public void Kill()
        {
            throw new System.NotImplementedException();
        }
    }

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
        public abstract string[] ResourceExtensions { get; }

        public virtual HierarchicalTreeSettingsData Settings { get { return null; } }
        #endregion

        #region Class Methods
        internal ContentAccessRequest GetAccessRequest()
        {
            accessRequest = CreateContentAccessRequest();
            accessRequest.Setup(ResourceKeywords, ResourceExtensions, Settings);
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
