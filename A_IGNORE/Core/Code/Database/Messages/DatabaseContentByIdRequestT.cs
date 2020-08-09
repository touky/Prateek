namespace Mayfair.Core.Code.Database.Messages
{
    public class DatabaseContentByKeynameRequest<TResponseType> : DatabaseContentByKeynameRequest
        where TResponseType : DatabaseContentByKeynameResponse, new()
    {
        #region Class Methods
        public TResponseType GetResponse(bool requestFailed = false)
        {
            return GetResponse<TResponseType>(requestFailed);
        }

        //todo protected override ResponseCommand CreateNewResponse()
        //todo {
        //todo     return Create<TResponseType>();
        //todo }
        #endregion
    }
}
