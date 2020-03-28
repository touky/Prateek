namespace Mayfair.Core.Code.VisualAsset
{
    using Mayfair.Core.Code.Resources.Loader;

    public interface IAssignableVisualResource
    {
        #region Properties
        //todo benjaminh: This is ugly and bad, and it should change
        int AssignmentIndex { get; }
        #endregion

        #region Class Methods
        void Assign<TResourceReference>(TResourceReference reference)
            where TResourceReference : class, IAbstractResourceReference;
        #endregion
    }
}
