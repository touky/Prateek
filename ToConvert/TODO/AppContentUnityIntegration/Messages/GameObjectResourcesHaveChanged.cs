namespace Mayfair.Core.Code.Resources.Messages
{
    using Mayfair.Core.Code.Resources.Loader;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged notice
    /// </summary>
    /// <typeparam name="TResourceType">The type of the resource</typeparam>
    public abstract class GameObjectResourcesHaveChanged : ResourcesHaveChangedResponse<GameObjectContentHandle, GameObject>
    {
        #region Class Methods
        protected override GameObjectContentHandle GetResourceRef(ContentLoader loader)
        {
            return new GameObjectContentHandle(loader);
        }
        #endregion
    }
}
