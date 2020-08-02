namespace Mayfair.Core.Code.SaveGame.Messages
{
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

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
        public override void Init(RequestCommand request)
        {
            base.Init(request);

            Debug.Assert(request is LoadDataRequest);

            this.request = (LoadDataRequest)request;
        }
        #endregion
    }
}
