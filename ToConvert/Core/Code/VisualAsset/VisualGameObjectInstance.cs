namespace Mayfair.Core.Code.VisualAsset
{
    using Mayfair.Core.Code.Resources.Loader;

    public abstract class VisualGameObjectInstance<TResourceReference>
        : VisualIdentifiableInstance, IAssignableVisualResource<TResourceReference>
        where TResourceReference : class, IAbstractResourceReference
    {
        #region IAssignableVisualResource<TResourceReference> Members
        public abstract int AssignmentIndex { get; }

        public abstract void Assign(TResourceReference reference);

        public void Assign<THigherResourceReference>(THigherResourceReference reference)
            where THigherResourceReference : class, IAbstractResourceReference
        {
            Assign(reference as TResourceReference);
        }
        #endregion
    }
}
