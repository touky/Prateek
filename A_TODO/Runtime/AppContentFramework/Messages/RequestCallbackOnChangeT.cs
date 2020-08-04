namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using System;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public abstract class RequestAccessToContent<TResourceType, TResponse, TIdentification>
        : RequestAccessToContent<TResponse, TIdentification>
        where TResponse : ResponseCommand, new()
        where TIdentification : Command
    {
        #region Properties
        protected override Type ResourceType
        {
            get { return typeof(TResourceType); }
        }
        #endregion
    }
}
