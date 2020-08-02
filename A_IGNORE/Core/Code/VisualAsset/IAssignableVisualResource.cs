namespace Mayfair.Core.Code.VisualAsset
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces;

    public interface IAssignableVisualResource
    {
        #region Properties
        //todo benjaminh: This is ugly and bad, and it should change
        int AssignmentIndex { get; }
        #endregion

        #region Class Methods
        void Assign<TResourceReference>(TResourceReference reference)
            where TResourceReference : class, IContentHandle;
        #endregion
    }
}
