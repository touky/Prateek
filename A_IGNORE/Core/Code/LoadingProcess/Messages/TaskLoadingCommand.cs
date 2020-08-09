namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public class TaskLoadingCommand : DirectCommand
    {
        #region Fields
        public LoadingTaskTracker trackerState;
        #endregion
    }
}
