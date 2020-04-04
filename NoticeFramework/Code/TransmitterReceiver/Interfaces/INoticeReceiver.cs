namespace Prateek.NoticeFramework.TransmitterReceiver
{
    using Prateek.NoticeFramework.Notices.Core;

    public interface INoticeReceiver : INoticeTransmitter
    {
        #region Class Methods
        //Sending
        void Send(TargetedNotice notice);
        void CleanUp();
        //Retrieving
        bool HasNotice();
        void ProcessAllNotices();
        #endregion

        #region Callbacks
        void AddCallback<T>(NoticeCallback<T> noticeCallback) where T : Notice;
        void RemoveCallback<T>() where T : Notice;
        void ClearCallbacks();
        void ApplyCallbacks();
        #endregion
    }
}

