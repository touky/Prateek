namespace Prateek.Runtime.AppContentFramework.Messages
{
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Commands.Core.Commands;
    using Prateek.Runtime.CommandFramework.Commands.Core.Interfaces;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;
    using UnityEngine.Assertions;

    public abstract class ContentAccessChangedResponse
        : ResponseCommand
        , IHierarchicalTreeSearchResult
    {
        #region Fields
        internal ContentAccessRequest request;
        #endregion

        #region Class Methods
        public override void Init(IRequestCommand request, bool requestFailed)
        {
            base.Init(request, requestFailed);

            this.request = request as ContentAccessRequest;

            Assert.IsNotNull(this.request);
        }
        #endregion

        #region IHierarchicalTreeSearchResult Members
        public abstract void Add(IHierarchicalTreeLeaf leaf);
        #endregion

        #region Nested type: Storage
        internal class Storage { }
        #endregion
    }
}
