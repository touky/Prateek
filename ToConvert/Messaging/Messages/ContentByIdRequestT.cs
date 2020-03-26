namespace Assets.Prateek.ToConvert.Messaging.Messages
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class ContentByIdRequest<TMessageId> : ContentByIdRequest
    {
        #region Properties
        public override long MessageID
        {
            get { return ConvertToId(typeof(TMessageId)); }
        }
        #endregion
    }
}
