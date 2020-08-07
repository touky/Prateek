namespace Prateek.Runtime.TickableFramework.Interfaces
{
    /// <summary>
    ///     Executed just before the FixedUpdate pass
    /// </summary>
    public interface IEarlyUpdateTickable : ITickable
    {
        #region Class Methods
        void EarlyUpdate();
        #endregion
    }
}
