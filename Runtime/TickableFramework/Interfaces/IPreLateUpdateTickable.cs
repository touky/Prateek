namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;

    /// <summary>
    ///     Executed just before the LateUpdate pass
    /// </summary>
    public interface IPreLateUpdateTickable
        : ITickable
        , IPriority<IPreLateUpdateTickable>
    {
        #region Class Methods
        void PreLateUpdate();
        #endregion
    }
}
