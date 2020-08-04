namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.CommandFramework.Servants;

    /// <summary>
    /// A targeted command that expect the recipient to send a response
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class RequestCommand<TResponse, TIdentification> : TargetedCommand, IRequestCommand
        where TResponse : ResponseCommand, new()
        where TIdentification : Command
    {
        public override CommandId CommandId
        {
            get { return typeof(TIdentification); }
        }

        #region Class Methods
        public ResponseCommand GetResponse(bool requestFailed = false)
        {
            var response = CreateNewResponse();
            response.Init(this, requestFailed);
            return response;
        }

        protected virtual TResponse CreateNewResponse()
        {
            return Command.Create<TResponse>();
        }
        #endregion
    }
}
