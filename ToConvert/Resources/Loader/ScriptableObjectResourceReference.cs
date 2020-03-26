namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.resource.ToString()}, Location: {loader.location}")]
    public class ScriptableObjectResourceReference<TScriptableResourceType> : UnityResourceReference<TScriptableResourceType, ScriptableObjectResourceReference<TScriptableResourceType>>
        where TScriptableResourceType : ScriptableObject
    {
        #region Properties
        public TScriptableResourceType Resource
        {
            get { return TypedResource; }
        }
        #endregion

        #region Constructors
        public ScriptableObjectResourceReference(ResourceLoader loader) : base(loader) { }
        #endregion
    }
}
