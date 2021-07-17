namespace Prateek.Runtime.CommandFramework.Tools
{
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Commands.Core.Commands;
    using Prateek.Runtime.CommandFramework.Gadgets;

    public struct BottledCommandBroadcaster<TNotice>
        where TNotice : BroadcastCommand, new()
    {
        #region Fields
        private CommandTools.IEmitter emitter;
        #endregion

        #region Constructors
        public BottledCommandBroadcaster(CommandTools.IEmitter emitter)
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
