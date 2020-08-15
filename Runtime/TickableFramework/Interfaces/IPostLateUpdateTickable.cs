namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;

    /// <summary>
    ///     Executed just before the LateUpdate pass
    /// </summary>
    public interface IPostLateUpdateTickable
        : ITickable
        , IPriority<IPostLateUpdateTickable>
    {
        #region Class Methods
        void PostLateUpdate();
        #endregion
    }
}
