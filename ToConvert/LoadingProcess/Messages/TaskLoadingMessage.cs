namespace Assets.Prateek.ToConvert.LoadingProcess.Messages
{
    using Assets.Prateek.ToConvert.Messaging.Messages;

    public class TaskLoadingMessage : DirectMessage
    {
        #region Fields
        public LoadingTaskTracker trackerState;
        #endregion
    }
}
