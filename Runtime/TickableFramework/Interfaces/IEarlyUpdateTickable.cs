namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;

    /// <summary>
    ///     Executed just before the FixedUpdate pass
    /// </summary>
    public interface IEarlyUpdateTickable
        : ITickable
        , IPriority<IEarlyUpdateTickable>
    {
        #region Class Methods
        void EarlyUpdate();
        #endregion
    }
}
