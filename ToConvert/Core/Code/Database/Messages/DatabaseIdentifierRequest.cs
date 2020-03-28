namespace Mayfair.Core.Code.Database.Messages
{
    using Messaging.Messages;

    public class DatabaseIdentifierRequest<TResponseType> : RequestMessage<TResponseType>
        where TResponseType : DatabaseIdentifierResponse, new()
    {
        
    }
}