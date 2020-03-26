namespace Assets.Prateek.ToConvert.Resources.Messages
{
    using Assets.Prateek.ToConvert.Resources.Loader;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged message
    /// </summary>
    /// <typeparam name="TResourceType">The type of the resource</typeparam>
    public abstract class ScriptableResourcesHaveChanged<TResourceType> : ResourcesHaveChangedResponse<ScriptableObjectResourceReference<TResourceType>, TResourceType>
        where TResourceType : ScriptableObject
    {
        #region Class Methods
        protected override ScriptableObjectResourceReference<TResourceType> GetResourceRef(ResourceLoader loader)
        {
            return new ScriptableObjectResourceReference<TResourceType>(loader);
        }
        #endregion
    }
}
