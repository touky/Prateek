namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables.ContentLoaders;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TContentType).Name}, {loader.content.ToString()}, Location: {loader.path}")]
    public abstract class UnityContentHandle<TContentType, TContentHandle>
        : ContentHandle<TContentType, TContentHandle>
        where TContentType : Object
        where TContentHandle : ContentHandle<TContentType, TContentHandle>
    {
        #region Class Methods
        public override void Init(ContentLoader loader)
        {
            base.Init(loader);

            loader.SetLoadContextParameters(new AddressableLoaderParameters
            {
                behaviour = LoaderBehaviour.Asset
            });
        }
        #endregion
    }
}
