namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Commands.Core;

    public class TaskLoadingCommand : DirectCommand
    {
        #region Fields
        public LoadingTaskTracker trackerState;
        #endregion
    }
}
