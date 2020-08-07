namespace Prateek.Runtime.TickableFramework.Interfaces
{
    /// <summary>
    ///     Use this to get callbacks from the application
    /// </summary>
    public interface IApplicationFeedbackTickable : ITickable
    {
        #region Class Methods
        void ApplicationQuit();
        void ApplicationFocus(bool focusStatus);
        void ApplicationPause(bool pauseStatus);
        #endregion
    }
}
