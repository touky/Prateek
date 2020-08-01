namespace Commands
{
    using System.Collections.Generic;
    using Commands.Core;

    public abstract class ContentByIdResponse<TContent> : ResponseCommand
    {
        #region Properties
        public List<TContent> Content { get; } = new List<TContent>();
        #endregion
    }
}
