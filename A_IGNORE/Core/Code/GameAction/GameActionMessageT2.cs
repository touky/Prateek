namespace Mayfair.Core.Code.GameAction
{
    using Prateek.KeynameFramework;
    using Prateek.KeynameFramework.Interfaces;
    using Prateek.NoticeFramework;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class GameActionNotice<T0, T1> : GameActionNotice
        where T0 : MasterKeyword
        where T1 : MasterKeyword
    {
        #region Constructors
        public GameActionNotice()
        {
            //tags.Add(Keyname.Create<T0>());
            //tags.Add(Keyname.Create<T1>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(INoticeTransmitter transmitter, float targetValue = 1)
        {
            GameActionNotice<T0, T1> notice = Create<GameActionNotice<T0, T1>>();
            notice.targetValue = targetValue;
            NoticeDaemonCore.DefaultNoticeTransmitter.Broadcast(notice);
        }

        public static void Broadcast(INoticeTransmitter transmitter, Keyname id0, float targetValue = 1)
        {
            GameActionNotice<T0, T1> notice = Create<GameActionNotice<T0, T1>>();
            notice.targetValue = targetValue;
            notice.Add(id0);
            NoticeDaemonCore.DefaultNoticeTransmitter.Broadcast(notice);
        }
        #endregion
    }
}
