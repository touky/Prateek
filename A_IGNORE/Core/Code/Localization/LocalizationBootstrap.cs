using Mayfair.Core.Code.BaseBehaviour;
using Mayfair.Core.Code.LoadingProcess;
using Mayfair.Core.Code.LoadingProcess.Enums;
using Mayfair.Core.Code.LoadingProcess.Messages;

namespace Mayfair.Core.Code.Localization
{
    using System.Collections;
    using System.Collections.Generic;
    using Commands.Core;
    using UnityEngine;

    public class LocalizationBootstrap : CommandReceiverOwner
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        protected override void SetupCommandReceiverCallback()
        {
            CommandReceiver.AddCallback<GameLoadingPrerequisiteCommand>(OnGameLoadingPrerequisiteNoticeReceived);
        }

        private void OnGameLoadingPrerequisiteNoticeReceived(GameLoadingPrerequisiteCommand command)
        {
            TaskLoadingCommand taskLoadingCommand = Command.Create<TaskLoadingCommand>();

            if (!LocalizationDaemon.SetLanguage(SystemLanguage.English, false))
            {
                throw new MissingFullLocalizationException(SystemLanguage.English);
            }

            taskLoadingCommand.trackerState = new LoadingTaskTracker(GetType(), LoadingTrackingStatus.HasLoadedPrerequisite);
            CommandReceiver.Send(taskLoadingCommand);
        }
        #endregion
    }
}