namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Interfaces.IPriority;

    public abstract class GadgetTools
    {
        public interface IInstantiator
            : IPriority
        {
            #region Class Methods
            void Declare(IInstantiatorBinder binder);
            void Bind(IGadgetBinder binder);
            #endregion
        }

        public interface IGadget
        {
            #region Class Methods
            void Awake();
            void Kill();
            #endregion
        }

        public interface IOwner
            : IAutoRegister
        {
            #region Properties
            string Name { get; }
            IGadgetPouch GadgetPouch { get; }
            #endregion
        }
    }
}