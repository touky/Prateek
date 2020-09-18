namespace Prateek.Runtime.CommandFramework.Commands.ContentById
{
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Runtime.CommandFramework.Commands.ContentById.Interfaces;
    using Prateek.Runtime.CommandFramework.Commands.Core;

    /// <summary>
    ///     Base response for any system that implement ContentByKeynameRequest
    /// </summary>
    /// <typeparam name="TContent"></typeparam>
    public abstract class ContentByKeynameResponse<TContent>
        : ResponseCommand, IContentByKeynameResponse
    {
        #region Fields
        public List<TContent> content = new List<TContent>();
        #endregion

        #region Properties
        public List<TContent> Content { get { return content; } }
        #endregion

        #region IContentByKeynameResponse Members
        IList IContentByKeynameResponse.Content { get { return content; } }
        #endregion
    }
}
