namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using Prateek.A_TODO.Runtime.AppContentFramework.ResourceTree;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.Servants;

    public abstract class ResourcesHaveChangedResponse : ResponseCommand, ITreeIdentificationResult
    {
        #region Properties
        public override CommandId CommandId
        {
            get { return new CommandId(typeof(ResourcesHaveChangedResponse), Recipient); }
        }
        #endregion

        #region ITreeIdentificationResult Members
        public abstract bool Match(ITreeLeafLocator leafLocator);
        public abstract void Add(ITreeLeafLocator leafLocator);
        #endregion
    }
}
