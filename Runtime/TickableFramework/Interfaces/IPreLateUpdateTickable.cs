namespace Prateek.Runtime.TickableFramework.Interfaces
{
    /// <summary>
    ///     Executed just before the LateUpdate pass
    /// </summary>
    public interface IPreLateUpdateTickable : ITickable
    {
        #region Class Methods
        void PreLateUpdate();
        #endregion
    }
}
