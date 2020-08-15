namespace Prateek.Runtime.FrameRecorder
{
    using Prateek.Runtime.Core.Consts;
    using UnityEngine;

    public static class FrameRecorder__
    {
        #region Properties
        public static RecorderState RecorderState
        {
            get
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return FrameRecorderManager.StateType.Inactive;
                //return instance.State;
                return RecorderState.Inactive;// FrameRecorderRegistry.CurrentState.Inactive;
            }
            set
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return;
                //instance.State = value;
            }
        }

        ///---------------------------------------------------------------------
        public static bool PlaybackActive
        {
            get
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return false;
                //return instance.PlaybackActive;
                return false;
            }
        }

        ///---------------------------------------------------------------------
        public static int FrameCount
        {
            get
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return 0;
                //return instance.FrameCount;
                return 0;
            }
        }

        ///---------------------------------------------------------------------
        public static int MaxFrameRecorded
        {
            get
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return 0;
                //return instance.MaxFrameRecorded;
                return 0;
            }
            set
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return;
                //instance.MaxFrameRecorded = value;
            }
        }

        ///---------------------------------------------------------------------
        public static Vector2Int CurrentFrameRange
        {
            get
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return Vector2Int.zero;
                //return instance.CurrentFrameRange;
                return Vector2Int.zero;
            }
            set
            {
                //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
                //if (instance == null)
                //    return;
                //instance.CurrentFrameRange = value;
            }
        }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        public static void ClearHistory()
        {
            //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
            //if (instance == null)
            //    return;

            //instance.InternalClearHistory();
        }
        #endregion

        ///---------------------------------------------------------------------

        ///---------------------------------------------------------------------

        #region External Access
        public static void Register(FrameRecorderRegistry.IRecorderBase recorder)
        {
            //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
            //if (instance == null)
            //    return;

            //instance.Register(recorder);
        }

        ///---------------------------------------------------------------------
        public static void Unregister(FrameRecorderRegistry.IRecorderBase recorder)
        {
            //var instance = TickableRegistry.GetManager<FrameRecorderManager>();
            //if (instance == null)
            //    return;

            //instance.Unregister(recorder);
        }
        #endregion External Access
    }
}
