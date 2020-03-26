namespace Assets.Prateek.ToConvert.Resources.Messages
{
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.Resources.ResourceTree;

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
