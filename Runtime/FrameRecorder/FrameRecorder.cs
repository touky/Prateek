namespace Prateek.Runtime.FrameRecorder
{
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class FrameRecorder
        : IGadget
    {
        #region Fields
        ///-----------------------------------------------------------------
        private IFrameRecorderOwner owner;

        private IRecordedFrame defaultFrame;
        private IRecordedFrame activeFrame;

        private bool isPlayback;
        #endregion

        #region Properties
        public bool IsPlayback { get { return isPlayback; } }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public FrameRecorder(IFrameRecorderOwner owner)
        {
            this.owner = owner;

            owner.GetDefaultFrame(out defaultFrame);
        }
        #endregion

        #region Class Methods
        ///-----------------------------------------------------------------
        public IRecordedFrame NewFrame()
        {
            return defaultFrame.CloneEmpty();
        }

        ///-----------------------------------------------------------------
        public void Open(IRecordedFrame recordedFrame)
        {
            activeFrame = recordedFrame;
            activeFrame.Open();
        }

        ///-----------------------------------------------------------------
        public void Close(IRecordedFrame recordedFrame)
        {
            activeFrame.Close();
        }

        ///-----------------------------------------------------------------
        public void Play(IRecordedFrame recordedFrame, bool isPlayback)
        {
            this.isPlayback = isPlayback;
            activeFrame = recordedFrame;
            activeFrame.Play(isPlayback);
        }
        #endregion

        #region IGadget Members
        ///-----------------------------------------------------------------
        public void Kill()
        {
            FrameRecorderRegistry.Unregister(this);
        }
        #endregion
    }
}
