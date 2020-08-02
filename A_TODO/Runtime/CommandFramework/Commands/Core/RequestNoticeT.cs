namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class RequestCommand<TResponseType> : RequestCommand
        where TResponseType : ResponseCommand, new()
    {
        #region Class Methods
        public new TResponseType GetResponse()
        {
            return base.GetResponse() as TResponseType;
        }

        protected override ResponseCommand CreateNewResponse()
        {
            return Command.Create<TResponseType>();
        }
        #endregion
    }
}

