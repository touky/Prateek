namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.resource.ToString()}, Location: {loader.location}")]
    public class MonoBehaviourResourceReference<TResourceType> : UnityObjectResourceReference<TResourceType, MonoBehaviourResourceReference<TResourceType>>
        where TResourceType : MonoBehaviour
    {
        #region Constructors
        public MonoBehaviourResourceReference(ResourceLoader loader) : base(loader) { }
        #endregion

        #region Class Methods
        protected override GameObject GetGameObject(TResourceType resourceType)
        {
            return resourceType.gameObject;
        }
        #endregion
    }
}
