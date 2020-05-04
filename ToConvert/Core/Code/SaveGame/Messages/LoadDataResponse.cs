namespace Mayfair.Core.Code.SaveGame.Messages
{
    using System.Diagnostics;
    using Prateek.NoticeFramework.Notices.Core;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}, Recipient: {recipient.Owner.Name}")]
    public class LoadDataResponse : ResponseNotice
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
        public override void Init(RequestNotice request)
        {
            base.Init(request);

            Debug.Assert(request is LoadDataRequest);

            this.request = (LoadDataRequest)request;
        }
        #endregion
    }
}
