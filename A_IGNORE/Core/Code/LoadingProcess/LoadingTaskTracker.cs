namespace Mayfair.Core.Code.LoadingProcess
{
    using System;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public struct LoadingTaskTracker
    {
        private Type type;
        private float stepProgress;
        private LoadingTrackingStatus status;
        private double timeMarker;

        public Type Type
        {
            get { return this.type; }
        }

        public float StepProgress
        {
            get { return this.stepProgress; }
            set { this.stepProgress = value; }
        }

        public LoadingTrackingStatus Status
        {
            get { return this.status; }
            set
            {
                if (value == LoadingTrackingStatus.Finished)
                {
                    this.timeMarker = Duration;
                }

                this.status = value;
                
                if (this.status == LoadingTrackingStatus.StartedLoading)
                {
                    this.timeMarker = TimeStamp;
                }
            }
        }

        public double Duration
        {
            get
            {
                if (this.status == LoadingTrackingStatus.Finished)
                {
                    return this.timeMarker;
                }
                else if (this.status >= LoadingTrackingStatus.StartedLoading)
                {
                    return TimeStamp - this.timeMarker;
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
            this.stepProgress = 0;
            this.timeMarker = 0;

            this.timeMarker = TimeStamp;
        }
    }
}
