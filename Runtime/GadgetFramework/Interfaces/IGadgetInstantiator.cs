namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    using System.Runtime.InteropServices.ComTypes;
    using Prateek.Runtime.Core.Interfaces.IPriority;

    public interface IGadgetInstantiator
        : IPriority
    {
        #region Class Methods
        void Declare(IInstantiatorBinder binder);
        void Bind(IGadgetBinder binder);
        #endregion
    }
}
