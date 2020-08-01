namespace Prateek.CommandFramework.Tools
{
    using Commands.Core;
    using Prateek.CommandFramework.TransmitterReceiver;

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
