namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;

    /// <summary>
    ///     Use this to get callbacks from the application
    /// </summary>
    public interface IApplicationFeedbackTickable
        : ITickable
        , IPriority<IApplicationFeedbackTickable>
    {
        #region Class Methods
        void ApplicationQuit();
        void ApplicationFocus(bool focusStatus);
        void ApplicationPause(bool pauseStatus);
        #endregion
    }
}
