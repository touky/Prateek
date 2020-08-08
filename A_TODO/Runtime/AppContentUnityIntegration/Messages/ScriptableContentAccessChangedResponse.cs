namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged notice
    /// </summary>
    /// <typeparam name="TResourceType">The type of the resource</typeparam>
    public abstract class ScriptableContentAccessChangedResponse<TResourceType> : ContentAccessChangedResponse<ScriptableObjectContentHandle<TResourceType>, TResourceType>
        where TResourceType : ScriptableObject
    {
        #region Class Methods
        protected override ScriptableObjectContentHandle<TResourceType> GetResourceRef(ContentLoader loader)
        {
            return new ScriptableObjectContentHandle<TResourceType>(loader);
        }
        #endregion
    }
}
