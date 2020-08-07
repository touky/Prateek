namespace Prateek.Runtime.TickableFramework.Interfaces
{
    /// <summary>
    ///     Executed just before the Update pass
    /// </summary>
    public interface IPreUpdateTickable : ITickable
    {
        #region Class Methods
        void PreUpdate();
        #endregion
    }
}
