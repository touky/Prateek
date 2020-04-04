namespace Prateek.NoticeFramework.TransmitterReceiver
{
    using System.Diagnostics;
    using Prateek.NoticeFramework.Notices.Core;
    using UnityEngine.Assertions;

    public delegate void NoticeCallback<T>(T notice) where T : Notice;

    [DebuggerDisplay("Callback: {callback.Method.Name}, Type: {GetType().Name}")]
    public class NoticeCallbackProxy<T> : INoticeCallbackProxy
        where T : Notice
    {
        #region Fields
        private NoticeCallback<T> callback;
        #endregion

        #region Constructors
        public NoticeCallbackProxy(NoticeCallback<T> callback)
        {
            this.callback = callback;
        }
        #endregion

        #region IMessageCallbackProxy Members
        public void Invoke(Notice notice)
        {
            T typedNotice = notice as T;
            Assert.IsNotNull(typedNotice, $"typedMessage is null for: notice = {notice.GetType().Name}, T = {typeof(T).Name}");
            if (typedNotice != null && this.callback != null)
            {
                this.callback.Invoke(typedNotice);
            }
        }
        #endregion
    }
}
