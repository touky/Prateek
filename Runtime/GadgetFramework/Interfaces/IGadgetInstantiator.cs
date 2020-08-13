namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;

    public interface IGadgetInstantiator
        : IPriority
    {
        #region Class Methods
        void Create(IGadgetOwner owner);
        #endregion
    }
}
