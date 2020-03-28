namespace Mayfair.Core.Code.Database.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;

    public class DatabaseContentByIdRequest<TResponseType> : DatabaseContentByIdRequest
        where TResponseType : DatabaseContentByIdResponse, new()
    {
        #region Class Methods
        public new TResponseType GetResponse()
        {
            return base.GetResponse() as TResponseType;
        }

        protected override ResponseMessage CreateNewResponse()
        {
            return Create<TResponseType>();
        }
        #endregion
    }
}
