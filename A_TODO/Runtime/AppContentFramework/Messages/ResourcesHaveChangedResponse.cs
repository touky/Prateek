namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.Servants;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public abstract class ResourcesHaveChangedResponse : ResponseCommand, IHierarchicalTreeSearchResult
    {
        #region Properties
        public override CommandId CommandId
        {
            get { return new CommandId(typeof(ResourcesHaveChangedResponse), Recipient); }
        }
        #endregion

        #region ITreeIdentificationResult Members
        public abstract void Add(IHierarchicalTreeLeaf leaf);
        #endregion
    }
}
