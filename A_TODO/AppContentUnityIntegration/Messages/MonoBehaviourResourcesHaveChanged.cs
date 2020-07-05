namespace Mayfair.Core.Code.Resources.Messages
{
    using Mayfair.Core.Code.Resources.Loader;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged notice
    /// </summary>
    /// <typeparam name="TResourceType">The type of the resource</typeparam>
    public abstract class MonoBehaviourResourcesHaveChanged<TResourceType> : ResourcesHaveChangedResponse<MonoBehaviourContentHandle<TResourceType>, TResourceType>
        where TResourceType : MonoBehaviour
    {
        #region Class Methods
        protected override MonoBehaviourContentHandle<TResourceType> GetResourceRef(ContentLoader loader)
        {
            return new MonoBehaviourContentHandle<TResourceType>(loader);
        }
        #endregion
    }
}
