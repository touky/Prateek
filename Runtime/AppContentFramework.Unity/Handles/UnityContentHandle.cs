namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TContentType).Name}, {loader.content.ToString()}, Location: {loader.path}")]
    public abstract class UnityContentHandle<TContentType, TContentHandle>
        : ContentHandle<TContentType, TContentHandle>
        where TContentType : Object
        where TContentHandle : ContentHandle<TContentType, TContentHandle>
    {
        #region Constructors
        protected UnityContentHandle(ContentLoader loader) : base(loader)
        {
            loader.SetLoadContextParameters(new AddressableContextParameters
            {
                behaviour = LoaderBehaviour.Asset
            });
        }
        #endregion
    }
}
