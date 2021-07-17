namespace Prateek.Runtime.FrameRecorder
{
    using System.Collections.Generic;

    ///---------------------------------------------------------------------
    internal class RegistryFrame
    {
        #region Fields
        ///-----------------------------------------------------------------
        public Dictionary<FrameRecorder, FrameRecord.IRecordedFrame> frames = new Dictionary<FrameRecorder, FrameRecord.IRecordedFrame>();
        #endregion

        #region Class Methods
        ///-----------------------------------------------------------------
        public void Open(FrameRecorder recorder)
        {
            if (!frames.ContainsKey(recorder))
            {
                frames.Add(recorder, recorder.NewFrame());
            }
            else
            {
                frames[recorder].Recycle();
            }

            recorder.Open(frames[recorder]);
        }
        
        ///-----------------------------------------------------------------
        public void Close(FrameRecorder recorder)
        {
            recorder.Close(frames[recorder]);
        }

        ///-----------------------------------------------------------------
        public void Play(bool isPlayback)
        {
            foreach (var pair in frames)
            {
                pair.Key.Play(pair.Value, isPlayback);
            }
        }
        #endregion
    }
}
