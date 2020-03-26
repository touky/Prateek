namespace Assets.Prateek.ToConvert.Localization
{
    using Assets.Prateek.ToConvert.BaseBehaviour;
    using Assets.Prateek.ToConvert.LoadingProcess;
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;
    using Assets.Prateek.ToConvert.LoadingProcess.Messages;
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using UnityEngine;

    public class LocalizationBootstrap : CommunicatorBehaviour
    {
        #region Methods
        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<GameLoadingPrerequisiteNotice>(OnGameLoadingPrerequisiteNoticeReceived);
        }

        private void OnGameLoadingPrerequisiteNoticeReceived(GameLoadingPrerequisiteNotice notice)
        {
            var taskLoadingMessage = Message.Create<TaskLoadingMessage>();

            if (!LocalizationService.SetLanguage(SystemLanguage.English, false))
            {
                throw new MissingFullLocalizationException(SystemLanguage.English);
            }

            taskLoadingMessage.trackerState = new LoadingTaskTracker(GetType(), LoadingTrackingStatus.HasLoadedPrerequisite);
            Communicator.Send(taskLoadingMessage);
        }
        #endregion
    }
}
