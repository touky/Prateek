namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.Servants;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;
    using UnityEngine.Assertions;

    public abstract class ContentAccessChangedResponse
        : ResponseCommand
        , IHierarchicalTreeSearchResult
    {
        #region Fields
        protected ContentAccessRequest request;
        #endregion

        #region Properties
        public override CommandId CommandId
        {
            get { return new CommandId(typeof(ContentAccessChangedResponse), Recipient); }
        }
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
