namespace Prateek.Runtime.FrameRecorder
{
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public interface IFrameRecorderOwner
        : IGadgetOwner
    {
        void GetDefaultFrame(out IRecordedFrame defaultFrame);
    }
}
