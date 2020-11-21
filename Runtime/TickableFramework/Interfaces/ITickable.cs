namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Interfaces.IPriority;

    /// <summary>
    ///     Base interface for the tickable framework
    ///     Prateek player loop order:
    ///     - <seealso cref="IEarlyUpdateTickable"/>
    ///     - <seealso cref="IPreUpdateTickable"/>
    ///     - <seealso cref="IPreLateUpdateTickable"/>
    ///     - <seealso cref="IPostLateUpdateTickable"/>
    ///     - <seealso cref="IApplicationFeedbackTickable"/>
    /// </summary>
    public interface ITickable
        : IPriority
        , IAutoRegister { }
}
