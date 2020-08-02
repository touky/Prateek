namespace Prateek.A_TODO.Runtime.CommandFramework.Tools
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public struct NoticeBroadcaster<TNotice>
        where TNotice : BroadcastCommand, new()
    {
        #region Fields
        private ICommandEmitter transmitter;
        #endregion

        #region Constructors
        public NoticeBroadcaster(ICommandEmitter transmitter)
        {
            this.transmitter = transmitter;
        }
        #endregion

        #region Class Methods
        public void Broadcast()
        {
            TNotice notice = Command.Create<TNotice>();

            this.transmitter.Broadcast(notice);
        }
        #endregion
    }
}
