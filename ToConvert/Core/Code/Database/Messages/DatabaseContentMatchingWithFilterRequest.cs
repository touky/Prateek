namespace Mayfair.Core.Code.Database.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Database.Messages.RequestFilters;


    public class DatabaseContentMatchingWithFilterRequest<TResponseType> : RequestMessage<TResponseType>
        where TResponseType : DatabaseContentByIdResponse, new()
    {
        public List<string> Filters { get; set; } = new List<string>();

        public FilterLogicalOperators Operator { get; set; } = FilterLogicalOperators.AND;
    }
}