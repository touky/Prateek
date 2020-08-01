namespace Commands.Core
{
    using System.Diagnostics;
    using Prateek.CommandFramework.TransmitterReceiver;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ResponseCommand : Command
    {
        #region Fields
        private ICommandEmitter recipient = null;
        #endregion

        #region Properties
        public ICommandEmitter Recipient
        {
            get { return recipient; }
        }

        //We allow notice type spoofing for Children notices
        public override long CommandID
        {
            get { return ConvertToId(GetType(), recipient); }
        }
        #endregion

        #region Class Methods
        public virtual void Init(RequestCommand request)
        {
            recipient = request.Emitter;
        }
        #endregion
    }
}
