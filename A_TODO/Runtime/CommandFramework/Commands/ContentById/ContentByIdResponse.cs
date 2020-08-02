namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById
{
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public abstract class ContentByIdResponse<TContent> : ResponseCommand
    {
        #region Properties
        public List<TContent> Content { get; } = new List<TContent>();
        #endregion
    }
}
