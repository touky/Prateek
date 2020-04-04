namespace Mayfair.Core.Code.GameAction
{
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Prateek.NoticeFramework;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class GameActionNotice<T0, T1, T2> : GameActionNotice
        where T0 : MasterTag
        where T1 : MasterTag
        where T2 : MasterTag
    {
        #region Constructors
        public GameActionNotice()
        {
            tags.Add(Keyname.Create<T0>());
            tags.Add(Keyname.Create<T1>());
            tags.Add(Keyname.Create<T2>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(INoticeTransmitter transmitter, float targetValue = 1)
        {
            GameActionNotice<T0, T1, T2> notice = Create<GameActionNotice<T0, T1, T2>>();
            notice.targetValue = targetValue;
            NoticeDaemonCore.DefaultNoticeTransmitter.Broadcast(notice);
        }

        public static void Broadcast(INoticeTransmitter transmitter, Keyname uniqueId1, float targetValue = 1)
        {
            GameActionNotice<T0, T1, T2> notice = Create<GameActionNotice<T0, T1, T2>>();
            notice.targetValue = targetValue;
            NoticeDaemonCore.DefaultNoticeTransmitter.Broadcast(notice);
        }

        #endregion
    }
}
