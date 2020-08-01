namespace Mayfair.Core.Code.Database.Messages
{
    using Commands.Core;

    public class DatabaseIdentifierRequest<TResponseType> : RequestCommand<TResponseType>
        where TResponseType : DatabaseIdentifierResponse, new()
    {
        
    }
}