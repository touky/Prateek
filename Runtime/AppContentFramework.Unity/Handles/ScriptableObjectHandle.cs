namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.Loader;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TContentType).Name}, {loader.content.ToString()}, Location: {loader.path}")]
    public class ScriptableObjectHandle<TContentType>
        : UnityContentHandle<TContentType, ScriptableObjectHandle<TContentType>>
        where TContentType : ScriptableObject
    {
        #region Properties
        public TContentType Content { get { return TypedContent; } }
        #endregion

        #region Constructors
        public ScriptableObjectHandle(ContentLoader loader) : base(loader) { }
        #endregion
    }
}
