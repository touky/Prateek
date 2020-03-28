namespace Mayfair.Core.Code.Messaging.Messages
{
    using System.Collections.Generic;

    public abstract class ContentByIdResponse<TContent> : ResponseMessage
    {
        #region Properties
        public List<TContent> Content { get; } = new List<TContent>();
        #endregion
    }
}
