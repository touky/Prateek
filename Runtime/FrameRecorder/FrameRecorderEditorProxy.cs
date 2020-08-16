namespace Prateek.Runtime.FrameRecorder
{
    using Prateek.Runtime.Core.Consts;
    using UnityEngine;

#if UNITY_EDITOR
    public static class FrameRecorderEditorProxy
    {
        #region Properties
        public static RecorderState RecorderState
        {
            get
            {
                return FrameRecorderRegistry.EditorInstance.CurrentState;
            }
            set
            {
                FrameRecorderRegistry.EditorInstance.NextState = value;
            }
        }

        ///---------------------------------------------------------------------
        public static int FrameCount
        {
            get
            {
                return FrameRecorderRegistry.EditorInstance.FrameCount;
            }
        }

        ///---------------------------------------------------------------------
        public static int FrameCapacity
        {
            get
            {
                return FrameRecorderRegistry.EditorInstance.FrameCapacity;
            }
            set
            {
                FrameRecorderRegistry.EditorInstance.FrameCapacity = value;
            }
        }

        ///---------------------------------------------------------------------
        public static Vector2Int PlaybackRange
        {
            get
            {
                return FrameRecorderRegistry.EditorInstance.PlaybackRange;
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
#endif
}
