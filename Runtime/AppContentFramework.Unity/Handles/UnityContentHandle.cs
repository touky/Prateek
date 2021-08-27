namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables.ContentLoaders;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TContentType).Name}, {loader.content.ToString()}, Location: {loader.path}")]
    public abstract class UnityContentHandle<TContentType>
        : ContentHandle<TContentType>
        where TContentType : Object
    {
        #region Class Methods
        protected override void OnInit(ContentLoader loader)
        {
            loader.SetLoadContextParameters(new AddressableLoaderParameters { behaviour = LoaderBehaviour.Asset });
        }
        #endregion
    }
}
