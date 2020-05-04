namespace Prateek.NoticeFramework.Notices.Core
{
    using System.Diagnostics;
    using Prateek.NoticeFramework.TransmitterReceiver;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}")]
    public abstract class ResponseNotice : Notice
    {
        #region Fields
        private INoticeTransmitter recipient = null;
        #endregion

        #region Properties
        public INoticeTransmitter Recipient
        {
            get { return recipient; }
        }

        //We allow notice type spoofing for Children notices
        public override long NoticeID
        {
            get { return ConvertToId(GetType(), recipient); }
        }
        #endregion

        #region Class Methods
        public virtual void Init(RequestNotice request)
        {
            recipient = request.Transmitter;
        }
        #endregion
    }
}
