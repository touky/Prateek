namespace Prateek.Runtime.AppContentFramework.Messages
{
    using System;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public abstract class ContentAccessRequest
        : RequestCommand
        , IHierarchicalTreeSearch
    {
        #region Fields
        private HierarchicalTreeSettingsData settings = null;
        private string[] contentPaths;
        internal ContentAccessChangedResponse.Storage storage = null;
        #endregion

        #region Properties
        protected abstract Type ContentType { get; }
        #endregion

        #region Class Methods
        public void Init(string[] contentPaths, HierarchicalTreeSettingsData settings = null)
        {
            this.contentPaths = contentPaths;
            this.settings = settings;
        }

        protected override bool ValidateResponse()
        {
            return holder.Validate<ContentAccessChangedResponse>();
        }
        #endregion

        #region IHierarchicalTreeSearch Members
        public string[] SearchPaths
        {
            get { return contentPaths; }
        }

        public HierarchicalTreeSettingsData Settings
        {
            get { return null; }
        }

        public virtual bool AcceptLeaf(IHierarchicalTreeLeaf leaf)
        {
            return true;
        }
        #endregion
    }
}
