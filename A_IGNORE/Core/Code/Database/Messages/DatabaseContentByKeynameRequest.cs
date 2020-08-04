namespace Mayfair.Core.Code.Database.Messages
{
    using Mayfair.Core.Code.Database.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById;

    public abstract class DatabaseContentByKeynameRequest : ContentByKeynameRequest<DatabaseContentByKeynameResponse,DatabaseContentByKeynameResponse>
    {
        public new DatabaseContentByKeynameResponse GetResponse()
        {
            return base.GetResponse() as DatabaseContentByKeynameResponse;
        }
    }
}
