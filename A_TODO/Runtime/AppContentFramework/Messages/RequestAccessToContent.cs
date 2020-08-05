namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using System;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public abstract class RequestAccessToContent<TResponse, TIdentification>
        : RequestCommand<TResponse, TIdentification>
        , IHierarchicalTreeSearch
        where TResponse : ResponseCommand, new()
        where TIdentification : Command
    {
        #region Fields
        private HierarchicalTreeSettingsData settings = null;
        private string[] contentPaths;
        #endregion

        #region Properties
        protected abstract Type ResourceType { get; }
        #endregion

        #region Class Methods
        public void Init(string[] contentPaths, HierarchicalTreeSettingsData settings = null)
        {
            this.contentPaths = contentPaths;
            this.settings = settings;
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
