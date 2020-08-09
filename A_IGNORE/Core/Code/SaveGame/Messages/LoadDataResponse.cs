namespace Mayfair.Core.Code.SaveGame.Messages
{
    using System.Diagnostics;
    using Prateek.Runtime.CommandFramework.Commands.Core;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}, Recipient: {recipient.Owner.Name}")]
    public class LoadDataResponse : ResponseCommand
    {
        #region Fields
        private LoadDataRequest request;
        #endregion

        #region Properties
        public LoadDataRequest Request
        {
            get { return this.request; }
        }
        #endregion

        #region Constructors
        public override void Init(IRequestCommand request, bool requestFailed = false)
        {
            base.Init(request, requestFailed);

            Debug.Assert(request is LoadDataRequest);

            this.request = (LoadDataRequest)request;
        }
        #endregion
    }
}
