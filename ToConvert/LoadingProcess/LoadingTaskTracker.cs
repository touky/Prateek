namespace Assets.Prateek.ToConvert.LoadingProcess
{
    using System;
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;
    using UnityEditor;

#if UNITY_EDITOR

#endif

    public struct LoadingTaskTracker
    {
        private Type type;
        private float stepProgress;
        private LoadingTrackingStatus status;
        private double timeMarker;

        public Type Type
        {
            get { return type; }
        }

        public float StepProgress
        {
            get { return stepProgress; }
            set { stepProgress = value; }
        }

        public LoadingTrackingStatus Status
        {
            get { return status; }
            set
            {
                if (value == LoadingTrackingStatus.Finished)
                {
                    timeMarker = Duration;
                }

                status = value;

                if (status == LoadingTrackingStatus.StartedLoading)
                {
                    timeMarker = TimeStamp;
                }
            }
        }

        public double Duration
        {
            get
            {
                if (status == LoadingTrackingStatus.Finished)
                {
                    return timeMarker;
                }
                else if (status >= LoadingTrackingStatus.StartedLoading)
                {
                    return TimeStamp - timeMarker;
                }

                return 0;
            }
        }

        private double TimeStamp
        {
            get
            {
#if UNITY_EDITOR
                return EditorApplication.timeSinceStartup;
#else
                return Time.realtimeSinceStartup;
#endif
            }
        }

        public LoadingTaskTracker(Type type, LoadingTrackingStatus status)
        {
            this.type = type;
            this.status = status;
            stepProgress = 0;
            timeMarker = 0;

            timeMarker = TimeStamp;
        }
    }
}
