namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration
{
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.content.ToString()}, Location: {loader.location}")]
    public class MonoBehaviourContentHandle<TContentType> : UnityObjectContentHandle<TContentType, MonoBehaviourContentHandle<TContentType>>
        where TContentType : MonoBehaviour
    {
        #region Constructors
        public MonoBehaviourContentHandle(ContentLoader loader) : base(loader) { }

        protected override GameObject GetGameObject(TContentType resourceType)
        {
            return resourceType.gameObject;
        }
        #endregion
    }
}
