namespace Mayfair.Core.Code.Database.Messages
{
    using Prateek.Runtime.CommandFramework.Commands.ContentById;

    public abstract class DatabaseContentByKeynameRequest : ContentByKeynameRequest
    {
        #region Class Methods
        protected override bool ValidateResponse()
        {
            return holder.Validate<DatabaseContentByKeynameResponse>();
        }
        #endregion
    }
}
