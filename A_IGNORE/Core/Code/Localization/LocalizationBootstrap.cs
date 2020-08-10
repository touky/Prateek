using Mayfair.Core.Code.BaseBehaviour;
using Mayfair.Core.Code.LoadingProcess;
using Mayfair.Core.Code.LoadingProcess.Enums;
using Mayfair.Core.Code.LoadingProcess.Messages;

namespace Mayfair.Core.Code.Localization
{
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.GadgetFramework;
    using UnityEngine;

    public class LocalizationBootstrap : CommandReceiverOwner
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        public override void DefineReceptionActions(ICommandReceiver receiver)
        {
            receiver.SetActionFor<GameLoadingPrerequisiteCommand>(OnGameLoadingPrerequisiteNoticeReceived);
        }

        private void OnGameLoadingPrerequisiteNoticeReceived(GameLoadingPrerequisiteCommand command)
        {
            TaskLoadingCommand taskLoadingCommand = CommandHelper.Create<TaskLoadingCommand>();

            if (!LocalizationDaemon.SetLanguage(SystemLanguage.English, false))
            {
                throw new MissingFullLocalizationException(SystemLanguage.English);
            }

            taskLoadingCommand.trackerState = new LoadingTaskTracker(GetType(), LoadingTrackingStatus.HasLoadedPrerequisite);
            this.Get<ICommandReceiver>().Send(taskLoadingCommand);
        }
        #endregion
    }
}