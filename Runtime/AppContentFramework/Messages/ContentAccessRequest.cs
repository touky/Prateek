namespace Prateek.Runtime.AppContentFramework.Messages
{
    using System;
    using Prateek.Runtime.AppContentFramework.ContentAccess;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public class ContentAccessRequest
        : RequestCommand
        , IHierarchicalTreeSearch
    {
        #region Fields
        private ContentAccessSettings accessSettings;
        internal ContentAccessChangedResponse.Storage storage = null;
        #endregion

        #region Class Methods
        public void Setup(ContentAccessSettings accessSettings)
        {
            this.accessSettings = accessSettings;
        }

        protected override bool ValidateResponse()
        {
            return holder.Validate<ContentAccessChangedResponse>();
        }
        #endregion

        #region IHierarchicalTreeSearch Members
        public string[] SearchPaths
        {
            get { return accessSettings.ContentPaths; }
        }

        public string[] SearchExtensions
        {
            get { return accessSettings.ContentExtensions; }
        }

        public HierarchicalTreeSettingsData Settings
        {
            get { return accessSettings.Settings.UseAsOverride ? accessSettings.Settings : null; }
        }

        public virtual bool AcceptLeaf(IHierarchicalTreeLeaf leaf)
        {
            return true;
        }
        #endregion
    }
}
