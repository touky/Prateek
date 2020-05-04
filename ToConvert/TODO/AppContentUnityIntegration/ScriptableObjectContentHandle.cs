namespace Mayfair.Core.Code.Resources.Loader
{
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.content.ToString()}, Location: {loader.location}")]
    public class ScriptableObjectContentHandle<TContentType> : UnityContentHandle<TContentType, ScriptableObjectContentHandle<TContentType>>
        where TContentType : ScriptableObject
    {
        #region Properties
        public TContentType Content
        {
            get { return TypedContent; }
        }
        #endregion

        #region Constructors
        public ScriptableObjectContentHandle(ContentLoader loader) : base(loader) { }
        #endregion
    }
}
