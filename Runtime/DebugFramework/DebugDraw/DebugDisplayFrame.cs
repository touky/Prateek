namespace Prateek.Runtime.DebugFramework.DebugDraw
{
    using System.Collections.Generic;
    using Prateek.Runtime.FrameRecorder;

    ///---------------------------------------------------------------------
    internal struct DebugDisplayFrame
        : FrameRecord.IRecordedFrame
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

        public FrameRecord.IRecorder SourceRecorder => throw new System.NotImplementedException();

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

        public FrameRecord.IRecordedFrame CloneEmpty()
        {
            throw new System.NotImplementedException();
        }

        public void Play(bool isPlayback)
        {
            throw new System.NotImplementedException();
        }
    }
}
