namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Handles;
    using UnityEngine;

    public abstract class GameObjectContentAccessResponse
        : ContentAccessChangedResponse<GameObject, GameObjectHandle>
    {
        #region Class Methods
        protected override GameObjectHandle GetHandle(ContentLoader loader)
        {
            return new GameObjectHandle(loader);
        }
        #endregion
    }
}
