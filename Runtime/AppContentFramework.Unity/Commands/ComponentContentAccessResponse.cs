namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Handles;
    using UnityEngine;

    public abstract class ComponentContentAccessResponse<TContentType>
        : ContentAccessChangedResponse<GameObjectHandle, GameObject>
        where TContentType : Component
    {
        #region Class Methods
        protected override GameObjectHandle GetHandle(ContentLoader loader)
        {
            return new ComponentHandle<TContentType>(loader);
        }
        #endregion
    }
}