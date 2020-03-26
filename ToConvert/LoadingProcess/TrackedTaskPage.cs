namespace Assets.Prateek.ToConvert.LoadingProcess
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.DebugMenu;
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using Assets.Prateek.ToConvert.DebugMenu.Pages;
    using Assets.Prateek.ToConvert.Helpers;
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;
    using Assets.Prateek.ToConvert.LoadingProcess.Messages;
    using Assets.Prateek.ToConvert.Reflection;
    using UnityEngine;

    internal class TrackedTaskPage : DebugMenuPage<LoadingProcessService>
    {
        #region Fields
        private ReflectedField<List<LoadingTaskTracker>> trackers = "trackers";
        #endregion

        #region Constructors
        public TrackedTaskPage(LoadingProcessService owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(LoadingProcessService owner, DebugMenuContext context)
        {
            foreach (LoadingTaskTracker tracker in trackers.Value)
            {
                var field = GetField<LoadingTaskTrackerField>();
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

                var step     = 1f / (float) LoadingTrackingStatus.Finished;
                var progress = Mathf.Clamp01(tracker.StepProgress * step + (float) tracker.Status * step);

                Draw(context, progress);
            }
            #endregion
        }
        #endregion
    }
}
