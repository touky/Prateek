namespace Mayfair.Core.Code.GameAction
{
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Prateek.NoticeFramework;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class GameActionNotice<T> : GameActionNotice
        where T : MasterKeyword
    {
        #region Constructors
        public GameActionNotice()
        {
            tags.Add(Keyname.Create<T>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(INoticeTransmitter transmitter, Keyname id0, float targetValue = 1)
        {
            GameActionNotice<T> notice = Create<GameActionNotice<T>>();
            notice.targetValue = targetValue;
            notice.Add(id0);
            NoticeDaemonCore.DefaultNoticeTransmitter.Broadcast(notice);
        }

        public static void Broadcast(INoticeTransmitter transmitter, Keyname id0, Keyname id1, float targetValue = 1)
        {
            GameActionNotice<T> notice = Create<GameActionNotice<T>>();
            notice.targetValue = targetValue;
            notice.Add(id0);
            notice.Add(id1);
            NoticeDaemonCore.DefaultNoticeTransmitter.Broadcast(notice);
        }
        #endregion
    }
}
