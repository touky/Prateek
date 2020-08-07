namespace Prateek.Runtime.TickableFramework.Interfaces
{
    /// <summary>
    ///     Executed just before the LateUpdate pass
    /// </summary>
    public interface IPostLateUpdateTickable : ITickable
    {
        #region Class Methods
        void PostLateUpdate();
        #endregion
    }
}
