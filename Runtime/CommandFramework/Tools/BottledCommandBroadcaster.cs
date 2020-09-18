namespace Prateek.Runtime.CommandFramework.Tools
{
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;

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
            var notice = Command.Create<TNotice>();

            emitter.Send(notice);
        }
        #endregion
    }
}
