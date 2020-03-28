namespace Mayfair.Core.Code.Resources.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Resources.ResourceTree;

    public abstract class ResourcesHaveChangedResponse : ResponseMessage, ITreeIdentificationResult
    {
        #region Properties
        public override long MessageID
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
