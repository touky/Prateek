namespace Assets.Prateek.ToConvert.Resources.Messages
{
    using Assets.Prateek.ToConvert.Resources.Loader;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged message
    /// </summary>
    /// <typeparam name="TResourceType">The type of the resource</typeparam>
    public abstract class MonoBehaviourResourcesHaveChanged<TResourceType> : ResourcesHaveChangedResponse<MonoBehaviourResourceReference<TResourceType>, TResourceType>
        where TResourceType : MonoBehaviour
    {
        #region Class Methods
        protected override MonoBehaviourResourceReference<TResourceType> GetResourceRef(ResourceLoader loader)
        {
            return new MonoBehaviourResourceReference<TResourceType>(loader);
        }
        #endregion
    }
}
