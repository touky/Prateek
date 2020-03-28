namespace Mayfair.Core.Code.LoadingProcess
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Fields;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.Utils.Helpers;
    using UnityEngine;

    internal class TrackedTaskPage : DebugMenuPage<LoadingProcessDaemonCore>
    {
        #region Fields
        private ReflectedField<List<LoadingTaskTracker>> trackers = "trackers";
        #endregion

        #region Constructors
        public TrackedTaskPage(LoadingProcessDaemonCore owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(LoadingProcessDaemonCore owner, DebugMenuContext context)
        {
            foreach (LoadingTaskTracker tracker in trackers.Value)
            {
                LoadingTaskTrackerField field = GetField<LoadingTaskTrackerField>();
                field.Draw(context, tracker);
            }
        }
        #endregion

        #region Nested type: LoadingTaskTrackerField
        private class LoadingTaskTrackerField : ProgressBarField
        {
            #region Constructors
            public LoadingTaskTrackerField() { }

            public LoadingTaskTrackerField(LoadingTaskTracker tracker) : base(tracker.Type.Name) { }
            #endregion

            #region Class Methods
            public void Draw(DebugMenuContext context, LoadingTaskTracker tracker)
            {
                if (tracker.Type.IsSubclassOf(typeof(GameLoadingNotice)))
                {
                    unloadedColor = ColorHelper.blue.pastel;
                    loadingColor = ColorHelper.blue.pastel;
                    loadedColor = ColorHelper.blue.pastel;
                }
                else
                {
                    ResetColors();
                }

                text = $"{tracker.Type.Name}: {tracker.Duration}s";

                float step = 1f / (float) LoadingTrackingStatus.Finished;
                float progress = Mathf.Clamp01(tracker.StepProgress * step + (float) tracker.Status * step);

                Draw(context, progress);
            }
            #endregion
        }
        #endregion
    }
}
