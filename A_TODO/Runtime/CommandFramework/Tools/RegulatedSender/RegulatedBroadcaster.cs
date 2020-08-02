namespace Prateek.A_TODO.Runtime.CommandFramework.Tools.RegulatedSender
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public class RegulatedBroadcaster<TNotice> : RegulatedNoticeSender<TNotice, ICommandEmitter>
        where TNotice : BroadcastCommand, new()
    {
        #region Constructors
        public RegulatedBroadcaster(ICommandEmitter transmitter) : base(transmitter) { }
        public RegulatedBroadcaster(ICommandEmitter transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Broadcast(nextMessage);
        }
        #endregion
    }
}
