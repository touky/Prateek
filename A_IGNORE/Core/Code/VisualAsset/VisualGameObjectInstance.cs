namespace Mayfair.Core.Code.VisualAsset
{
    using Prateek.Runtime.AppContentFramework.Loader.Interfaces;

    public abstract class VisualGameObjectInstance<TResourceReference>
        : VisualKeynameUserInstance, IAssignableVisualResource<TResourceReference>
        where TResourceReference : class, IContentHandle
    {
        #region IAssignableVisualResource<TResourceReference> Members
        public abstract int AssignmentIndex { get; }

        public abstract void Assign(TResourceReference reference);

        public void Assign<THigherResourceReference>(THigherResourceReference reference)
            where THigherResourceReference : class, IContentHandle
        {
            Assign(reference as TResourceReference);
        }
        #endregion
    }
}
