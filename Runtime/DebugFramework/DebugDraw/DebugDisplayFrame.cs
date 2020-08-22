namespace Prateek.Runtime.DebugFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.FrameRecorder;

    ///---------------------------------------------------------------------
    internal struct DebugDisplayFrame : IRecordedFrame
    {
        ///-----------------------------------------------------------------
        private DebugDisplayRegistry owner;
        private List<DebugPrimitiveSetup> framePrimitives;

        ///-----------------------------------------------------------------
        public FrameRecorderRegistry.IRecorderBase Owner { get { return owner; } }
        public List<DebugPrimitiveSetup> FramePrimitives
        {
            get
            {
                if (framePrimitives == null)
                    framePrimitives = new List<DebugPrimitiveSetup>();
                return framePrimitives;
            }
        }

        public FrameRecorder SourceRecorder => throw new System.NotImplementedException();

        ///-----------------------------------------------------------------
        public DebugDisplayFrame(DebugDisplayRegistry owner)
        {
            this.owner = owner;
            framePrimitives = new List<DebugPrimitiveSetup>();
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Recycle()
        {
            throw new System.NotImplementedException();
        }

        public IRecordedFrame CloneEmpty()
        {
            throw new System.NotImplementedException();
        }

        public void Play(bool isPlayback)
        {
            throw new System.NotImplementedException();
        }
    }
}
