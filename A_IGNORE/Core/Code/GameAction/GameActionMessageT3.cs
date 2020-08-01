namespace Mayfair.Core.Code.GameAction
{
    using Prateek.KeynameFramework;
    using Prateek.KeynameFramework.Interfaces;
    using Prateek.NoticeFramework;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class GameActionNotice<T0, T1, T2> : GameActionNotice
        where T0 : MasterKeyword
        where T1 : MasterKeyword
        where T2 : MasterKeyword
    {
        #region Constructors
        public GameActionNotice()
        {
            //tags.Add(Keyname.Create<T0>());
            //tags.Add(Keyname.Create<T1>());
            //tags.Add(Keyname.Create<T2>());
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
