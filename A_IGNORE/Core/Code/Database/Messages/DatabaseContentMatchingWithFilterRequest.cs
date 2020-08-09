namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Database.Messages.RequestFilters;
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public class DatabaseContentMatchingWithFilterRequest<TResponseType> : RequestCommand
        where TResponseType : DatabaseContentByKeynameResponse, new()
    {
        public List<string> Filters { get; set; } = new List<string>();

        public FilterLogicalOperators Operator { get; set; } = FilterLogicalOperators.AND;

        protected override bool ValidateResponse()
        {
            return holder.Validate<DatabaseContentByKeynameResponse>();
        }
    }
}