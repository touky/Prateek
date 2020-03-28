namespace Mayfair.Core.Code.SaveGame.Messages
{
    using System.Diagnostics;
    using Mayfair.Core.Code.Messaging.Messages;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}, Recipient: {recipient.Owner.Name}")]
    public class LoadDataResponse : ResponseMessage
    {
        #region Fields
        private LoadDataRequest request;
        #endregion

        #region Properties
        public LoadDataRequest Request
        {
            get { return this.request; }
        }
        #endregion

        #region Constructors
        public override void Init(RequestMessage request)
        {
            base.Init(request);

            Debug.Assert(request is LoadDataRequest);

            this.request = (LoadDataRequest)request;
        }
        #endregion
    }
}
