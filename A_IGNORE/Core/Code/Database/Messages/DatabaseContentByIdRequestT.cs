namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class DatabaseContentByIdRequest<TResponseType> : DatabaseContentByIdRequest
        where TResponseType : DatabaseContentByIdResponse, new()
    {
        #region Class Methods
        public new TResponseType GetResponse()
        {
            return base.GetResponse() as TResponseType;
        }

        protected override ResponseCommand CreateNewResponse()
        {
            return Create<TResponseType>();
        }
        #endregion
    }
}
