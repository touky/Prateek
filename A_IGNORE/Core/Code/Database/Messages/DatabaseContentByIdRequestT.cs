namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class DatabaseContentByKeynameRequest<TResponseType> : DatabaseContentByKeynameRequest
        where TResponseType : DatabaseContentByKeynameResponse, new()
    {
        #region Class Methods
        public new TResponseType GetResponse()
        {
            return base.GetResponse() as TResponseType;
        }

        //todo protected override ResponseCommand CreateNewResponse()
        //todo {
        //todo     return Create<TResponseType>();
        //todo }
        #endregion
    }
}
