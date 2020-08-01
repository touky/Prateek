namespace Prateek.CommandFramework.Tools
{
    using Commands.Core;
    using Prateek.CommandFramework.TransmitterReceiver;

    public class RegulatedTargetedSender<TNotice> : RegulatedNoticeSender<TNotice, ICommandReceiver>
        where TNotice : TargetedCommand, new()
    {
        #region Constructors
        public RegulatedTargetedSender(ICommandReceiver transmitter) : base(transmitter) { }
        public RegulatedTargetedSender(ICommandReceiver transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Send(nextMessage);
        }
        #endregion
    }
}
