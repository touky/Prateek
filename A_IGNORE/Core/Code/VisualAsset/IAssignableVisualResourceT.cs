namespace Mayfair.Core.Code.VisualAsset
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces;

    public interface IAssignableVisualResource<TResourceReference> : IAssignableVisualResource
        where TResourceReference : class, IContentHandle
    {
        #region Class Methods
        void Assign(TResourceReference reference);
        #endregion
    }
}
