namespace Assets.Prateek.ToConvert.SaveGame.Messages
{
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.Messaging.Messages;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}, Recipient: {recipient.Owner.Name}")]
    public class LoadDataResponse : ResponseMessage
    {
        #region Fields
        private LoadDataRequest request;
        #endregion

        #region Properties
        public LoadDataRequest Request
        {
            get { return request; }
        }
        #endregion

        #region Class Methods
        public override void Init(RequestMessage request)
        {
            base.Init(request);

            Debug.Assert(request is LoadDataRequest);

            this.request = (LoadDataRequest) request;
        }
        #endregion
    }
}
