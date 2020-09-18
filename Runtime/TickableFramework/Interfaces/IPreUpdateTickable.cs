namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;

    /// <summary>
    ///     Executed just before the Update pass
    /// </summary>
    public interface IPreUpdateTickable
        : ITickable
        , IPriority<IPreUpdateTickable>
    {
        #region Class Methods
        void PreUpdate();
        #endregion
    }
}
