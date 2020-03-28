namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;

    public class TaskLoadingMessage : DirectMessage
    {
        #region Fields
        public LoadingTaskTracker trackerState;
        #endregion
    }
}
