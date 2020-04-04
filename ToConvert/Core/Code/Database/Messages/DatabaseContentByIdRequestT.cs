namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.NoticeFramework.Notices.Core;

    public class DatabaseContentByIdRequest<TResponseType> : DatabaseContentByIdRequest
        where TResponseType : DatabaseContentByIdResponse, new()
    {
        #region Class Methods
        public new TResponseType GetResponse()
        {
            return base.GetResponse() as TResponseType;
        }

        protected override ResponseNotice CreateNewResponse()
        {
            return Create<TResponseType>();
        }
        #endregion
    }
}
