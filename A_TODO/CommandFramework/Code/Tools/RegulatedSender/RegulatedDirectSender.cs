namespace Prateek.CommandFramework.Tools
{
    using Commands.Core;
    using Prateek.CommandFramework.TransmitterReceiver;

    public class RegulatedDirectSender<TNotice> : RegulatedNoticeSender<TNotice, ICommandEmitter>
        where TNotice : DirectCommand, new()
    {
        #region Constructors
        public RegulatedDirectSender(ICommandEmitter transmitter) : base(transmitter) { }
        public RegulatedDirectSender(ICommandEmitter transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Send(nextMessage);
        }
        #endregion
    }
}
