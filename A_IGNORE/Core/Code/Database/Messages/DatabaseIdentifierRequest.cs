namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.NoticeFramework.Notices.Core;

    public class DatabaseIdentifierRequest<TResponseType> : RequestNotice<TResponseType>
        where TResponseType : DatabaseIdentifierResponse, new()
    {
        
    }
}