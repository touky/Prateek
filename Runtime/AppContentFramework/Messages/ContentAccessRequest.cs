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
        private string[] contentExtensions;
        internal ContentAccessChangedResponse.Storage storage = null;
        #endregion

        #region Properties
        protected abstract Type ContentType { get; }
        #endregion

        #region Class Methods
        public void Setup(string[] contentPaths, string[] contentExtensions = null, HierarchicalTreeSettingsData settings = null)
        {
            this.contentPaths = contentPaths;
            this.contentExtensions = contentExtensions;
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

        public string[] SearchExtensions
        {
            get { return contentExtensions; }
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
