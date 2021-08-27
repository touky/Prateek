namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Handles;
    using UnityEngine;

    public abstract class ComponentContentAccessResponse<TContentType>
        : ContentAccessChangedResponse<GameObjectHandle>
        where TContentType : Component
    {
        #region Class Methods
        protected override GameObjectHandle GetHandle()
        {
            return new ComponentHandle<TContentType>();
        }
        #endregion
    }
}
