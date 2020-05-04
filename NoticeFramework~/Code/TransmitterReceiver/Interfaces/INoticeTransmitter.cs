namespace Prateek.NoticeFramework.TransmitterReceiver
{
    using Prateek.NoticeFramework.Notices.Core;

    public interface INoticeTransmitter
    {
        #region Properties
        INoticeReceiverOwner Owner { get; }
        #endregion

        #region Class Methods
        //Sending
        void Broadcast(BroadcastNotice notice);
        void Send(DirectNotice notice);
        void Send(ResponseNotice notice);
        #endregion
    }
}
