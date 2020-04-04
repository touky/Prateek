namespace Mayfair.Core.Code.Resources.Messages
{
    using Mayfair.Core.Code.Resources.ResourceTree;
    using Prateek.NoticeFramework.Notices.Core;

    public abstract class ResourcesHaveChangedResponse : ResponseNotice, ITreeIdentificationResult
    {
        #region Properties
        public override long NoticeID
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
