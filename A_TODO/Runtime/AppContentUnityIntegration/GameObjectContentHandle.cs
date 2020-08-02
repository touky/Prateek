namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration
{
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using UnityEngine;

    [DebuggerDisplay("{loader.content.ToString()}, Location: {loader.location}")]
    public class GameObjectContentHandle : UnityObjectContentHandle<GameObject, GameObjectContentHandle>
    {
        #region Constructors
        public GameObjectContentHandle(ContentLoader loader) : base(loader) { }

        protected override GameObject GetGameObject(GameObject resourceType)
        {
            return resourceType;
        }
        #endregion
    }
}
