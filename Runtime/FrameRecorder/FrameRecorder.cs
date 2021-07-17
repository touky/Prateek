namespace Prateek.Runtime.FrameRecorder
{
    using Prateek.Runtime.GadgetFramework.Interfaces;

    internal class FrameRecorder
        : FrameRecord.IRecorder
    {
        #region Fields
        ///-----------------------------------------------------------------
        private FrameRecord.IOwner Owner { get; set; }

        private FrameRecord.IRecordedFrame defaultFrame;
        private FrameRecord.IRecordedFrame activeFrame;

        private bool isPlayback;
        #endregion

        #region Properties
        public bool IsPlayback { get { return isPlayback; } }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public FrameRecorder()
        {
        }
        #endregion

        #region Class Methods
        ///-----------------------------------------------------------------
        public FrameRecord.IRecordedFrame NewFrame()
        {
            return defaultFrame.CloneEmpty();
        }

        ///-----------------------------------------------------------------
        public void Open(FrameRecord.IRecordedFrame recordedFrame)
        {
            activeFrame = recordedFrame;
            activeFrame.Open();
        }

        ///-----------------------------------------------------------------
        public void Close(FrameRecord.IRecordedFrame recordedFrame)
        {
            activeFrame.Close();
        }

        ///-----------------------------------------------------------------
        public void Play(FrameRecord.IRecordedFrame recordedFrame, bool isPlayback)
        {
            this.isPlayback = isPlayback;
            activeFrame = recordedFrame;
            activeFrame.Play(isPlayback);
        }
        #endregion

        #region IGadget Members
        public void Awake()
        {
            Owner.GetDefaultFrame(out defaultFrame);

            FrameRecorderRegistry.Register(this);
        }

        public void Kill()
        {
            FrameRecorderRegistry.Unregister(this);
        }
        #endregion
    }
}
