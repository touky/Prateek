namespace Prateek.NoticeFramework.TransmitterReceiver
{
    using Prateek.NoticeFramework.Notices.Core;

    public interface INoticeCallbackProxy
    {
        #region Class Methods
        void Invoke(Notice notice);
        #endregion
    }
}
