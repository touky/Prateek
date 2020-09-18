namespace Prateek.Runtime.FrameRecorder
{
    ///-----------------------------------------------------------------
    public interface IRecordedFrame
    {
        #region Properties
        FrameRecorder SourceRecorder { get; }
        #endregion

        #region Class Methods
        IRecordedFrame CloneEmpty();
        void Open();
        void Close();
        void Recycle();
        void Play(bool isPlayback);
        #endregion
    }
}
