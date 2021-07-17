namespace Prateek.Runtime.GadgetFramework
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.GadgetFramework.Interfaces;

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