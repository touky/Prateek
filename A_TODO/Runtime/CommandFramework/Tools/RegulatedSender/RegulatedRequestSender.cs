namespace Prateek.A_TODO.Runtime.CommandFramework.Tools.RegulatedSender
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public class RegulatedRequestSender<TNotice> : RegulatedNoticeSender<TNotice, ICommandReceiver>
        where TNotice : DirectCommand, new()
    {
        #region Constructors
        public RegulatedRequestSender(ICommandReceiver transmitter) : base(transmitter) { }
        public RegulatedRequestSender(ICommandReceiver transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Send(nextMessage);
        }
        #endregion
    }
}
