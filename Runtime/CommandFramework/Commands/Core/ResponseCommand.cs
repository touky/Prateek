namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Servants;

    /// <summary>
    /// Sent as a result of a request, a response is sent to the receiver that sent the request
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ResponseCommand : Command
    {
        #region Fields
        private bool requestFailed = false;
        private ICommandEmitter recipient = null;
        #endregion

        #region Properties
        public bool RequestFailed
        {
            get { return requestFailed; }
        }

        public ICommandEmitter Recipient
        {
            get { return recipient; }
        }

        //We allow notice type spoofing for Children notices
        public override CommandId CommandId
        {
            get { return new CommandId(GetType(), recipient); }
        }
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
