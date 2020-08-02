namespace Mayfair.Core.Code.Database.Messages
{
    using Mayfair.Core.Code.Database.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById;

    public abstract class DatabaseContentByIdRequest : ContentByIdRequest<DatabaseContentByIdRequest>
    {
        public new DatabaseContentByIdResponse GetResponse()
        {
            return base.GetResponse() as DatabaseContentByIdResponse;
        }
    }
}
