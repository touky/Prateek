namespace Mayfair.Core.Code.Resources.Loader
{
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.TODO.AppContentFramework.Loader.Enums;
    using Mayfair.Core.Code.Resources.Enums;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.content.ToString()}, Location: {loader.location}")]
    public abstract class UnityContentHandle<TResourceType, TResourceReference>
        : ContentHandle<TResourceType, TResourceReference>
        where TResourceType : Object
        where TResourceReference : ContentHandle<TResourceType, TResourceReference>
    {
        #region Constructors
        protected UnityContentHandle(ContentLoader loader) : base(loader)
        {
            loader.Behaviour = LoaderBehaviour.Asset;
        }
        #endregion

        #region Class Methods
        /// <summary>
        ///     Loads the resource
        /// </summary>
        public override void LoadAsync()
        {
            if (loader.Status == AsyncStatus.Nothing)
            {
                content = default;
                loader.LoadCompleted = OnAsyncCompleted;

                InternalLoad();
            }
        }
        #endregion
    }
}
