namespace Prateek.Runtime.CommandFramework.Commands.Core.Commands
{
    using System.Diagnostics;
    using Prateek.Runtime.CommandFramework.Commands.Core.Interfaces;
    using Prateek.Runtime.CommandFramework.Gadgets;

    /// <summary>
    ///     Sent as a result of a request, a response is sent to the receiver that sent the request
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ResponseCommand : Command
    {
        #region Fields
        private bool requestFailed = false;
        private CommandTools.IEmitter recipient = null;
        #endregion

        #region Properties
        public bool RequestFailed { get { return requestFailed; } }

        public CommandTools.IEmitter Recipient { get { return recipient; } }

        //We allow notice type spoofing for Children notices
        public override CommandId CommandId { get { return new CommandId(GetType(), recipient); } }
        #endregion

        #region Class Methods
        public virtual void Init(IRequestCommand request, bool requestFailed)
        {
            recipient = request.Emitter;
            this.requestFailed = requestFailed;
        }
        #endregion
    }
}
