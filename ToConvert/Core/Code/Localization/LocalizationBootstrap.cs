using Mayfair.Core.Code.BaseBehaviour;
using Mayfair.Core.Code.LoadingProcess;
using Mayfair.Core.Code.LoadingProcess.Enums;
using Mayfair.Core.Code.LoadingProcess.Messages;

namespace Mayfair.Core.Code.Localization
{
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.NoticeFramework.Notices.Core;
    using UnityEngine;

    public class LocalizationBootstrap : NoticeReceiverOwner
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<GameLoadingPrerequisiteNotice>(OnGameLoadingPrerequisiteNoticeReceived);
        }

        private void OnGameLoadingPrerequisiteNoticeReceived(GameLoadingPrerequisiteNotice notice)
        {
            TaskLoadingNotice taskLoadingNotice = Notice.Create<TaskLoadingNotice>();

            if (!LocalizationDaemonCore.SetLanguage(SystemLanguage.English, false))
            {
                throw new MissingFullLocalizationException(SystemLanguage.English);
            }

            taskLoadingNotice.trackerState = new LoadingTaskTracker(GetType(), LoadingTrackingStatus.HasLoadedPrerequisite);
            NoticeReceiver.Send(taskLoadingNotice);
        }
        #endregion
    }
}