namespace Mayfair.Core.Code.Resources.Loader
{
    using System.Diagnostics;
    using Mayfair.Core.Code.Resources.Enums;
    using UnityEngine;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.resource.ToString()}, Location: {loader.location}")]
    public abstract class UnityResourceReference<TResourceType, TResourceReference> : AbstractResourceReference<TResourceType, TResourceReference>
        where TResourceType : Object
        where TResourceReference : AbstractResourceReference<TResourceType, TResourceReference>
    {
        #region Constructors
        protected UnityResourceReference(ResourceLoader loader) : base(loader)
        {
            loader.Behaviour = ResourceLoader.LoaderBehaviour.Asset;
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
                resource = default;
                loader.LoadCompleted = OnAsyncCompleted;

                InternalLoad();
            }
        }
        #endregion
    }
}
