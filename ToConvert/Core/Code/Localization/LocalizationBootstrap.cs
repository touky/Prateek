using Mayfair.Core.Code.BaseBehaviour;
using Mayfair.Core.Code.LoadingProcess;
using Mayfair.Core.Code.LoadingProcess.Enums;
using Mayfair.Core.Code.LoadingProcess.Messages;
using Mayfair.Core.Code.Messaging.Messages;

namespace Mayfair.Core.Code.Localization
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LocalizationBootstrap : CommunicatorBehaviour
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<GameLoadingPrerequisiteNotice>(OnGameLoadingPrerequisiteNoticeReceived);
        }

        private void OnGameLoadingPrerequisiteNoticeReceived(GameLoadingPrerequisiteNotice notice)
        {
            TaskLoadingMessage taskLoadingMessage = Message.Create<TaskLoadingMessage>();

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