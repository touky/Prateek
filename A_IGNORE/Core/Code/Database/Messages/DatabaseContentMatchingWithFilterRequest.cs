namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Database.Messages.RequestFilters;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class DatabaseContentMatchingWithFilterRequest<TResponseType> : RequestCommand<TResponseType, TResponseType>
        where TResponseType : DatabaseContentByKeynameResponse, new()
    {
        public List<string> Filters { get; set; } = new List<string>();

        public FilterLogicalOperators Operator { get; set; } = FilterLogicalOperators.AND;
    }
}