namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class DatabaseIdentifierRequest<TResponseType> : RequestCommand
        where TResponseType : DatabaseIdentifierResponse, new()
    {

        protected override bool ValidateResponse()
        {
            return holder.Validate<DatabaseIdentifierResponse>();
        }
    }
}