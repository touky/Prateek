namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class RequestCommand : TargetedCommand
    {
        #region Class Methods
        public ResponseCommand GetResponse()
        {
            ResponseCommand response = CreateNewResponse();
            response.Init(this);
            return response;
        }

        protected abstract ResponseCommand CreateNewResponse();
        #endregion
    }
}
