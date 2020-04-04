namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Database.Messages.RequestFilters;
    using Prateek.NoticeFramework.Notices.Core;

    public class DatabaseContentMatchingWithFilterRequest<TResponseType> : RequestNotice<TResponseType>
        where TResponseType : DatabaseContentByIdResponse, new()
    {
        public List<string> Filters { get; set; } = new List<string>();

        public FilterLogicalOperators Operator { get; set; } = FilterLogicalOperators.AND;
    }
}