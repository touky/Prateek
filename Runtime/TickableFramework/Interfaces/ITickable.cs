namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Interfaces.IPriority;

    /// <summary>
    ///     Base interface for the tickable framework
    /// </summary>
    public interface ITickable
        : IPriority
        , IAutoRegister { }
}
