namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    using Prateek.Runtime.Core.AutoRegistration;

    public interface IGadgetOwner
        : IAutoRegister
    {
        #region Properties
        string Name { get; }
        IGadgetPouch GadgetPouch { get; }
        #endregion
    }
}
