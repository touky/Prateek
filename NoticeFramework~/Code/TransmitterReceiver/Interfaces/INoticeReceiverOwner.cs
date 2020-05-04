namespace Prateek.NoticeFramework.TransmitterReceiver
{
    using UnityEngine;

    public interface INoticeReceiverOwner
    {
        #region Properties
        INoticeReceiver NoticeReceiver { get; }
        string Name { get; }
        Transform Transform { get; }
        #endregion

        #region Class Methods
        void NoticeReceived();
        #endregion
    }
}
