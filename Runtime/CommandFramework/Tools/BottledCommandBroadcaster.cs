namespace Prateek.A_TODO.Runtime.CommandFramework.Tools
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public struct BottledCommandBroadcaster<TNotice>
        where TNotice : BroadcastCommand, new()
    {
        #region Fields
        private ICommandEmitter emitter;
        #endregion

        #region Constructors
        public BottledCommandBroadcaster(ICommandEmitter emitter)
        {
            this.emitter = emitter;
        }
        #endregion

        #region Class Methods
        public void Broadcast()
        {
            TNotice notice = Command.Create<TNotice>();

            this.emitter.Send(notice);
        }
        #endregion
    }
}
