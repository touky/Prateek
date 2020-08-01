namespace Mayfair.Core.Code.Resources.Messages
{
    using Mayfair.Core.Code.Resources.ResourceTree;
    using Commands.Core;

    public abstract class ResourcesHaveChangedResponse : ResponseCommand, ITreeIdentificationResult
    {
        #region Properties
        public override long CommandID
        {
            get { return ConvertToId(typeof(ResourcesHaveChangedResponse), Recipient); }
        }
        #endregion

        #region ITreeIdentificationResult Members
        public abstract bool Match(ITreeLeafLocator leafLocator);
        public abstract void Add(ITreeLeafLocator leafLocator);
        #endregion
    }
}
