namespace Mayfair.Core.Code.VisualAsset
{
    using Mayfair.Core.Code.Resources.Loader;

    public interface IAssignableVisualResource<TResourceReference> : IAssignableVisualResource
        where TResourceReference : class, IAbstractResourceReference
    {
        #region Class Methods
        void Assign(TResourceReference reference);
        #endregion
    }
}
