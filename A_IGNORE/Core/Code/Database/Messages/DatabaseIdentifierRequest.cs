namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class DatabaseIdentifierRequest<TResponseType> : RequestCommand<TResponseType, TResponseType>
        where TResponseType : DatabaseIdentifierResponse, new()
    {
        
    }
}