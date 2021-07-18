namespace Prateek.Runtime.FrameRecorder
{
    using JetBrains.Annotations;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public abstract class FrameRecord
        : GadgetTools
    {
        [UsedImplicitly]
        internal class Instantiator
            : IInstantiator
        {
            #region IGadgetInstantiator Members
            public int DefaultPriority { get { return typeof(Instantiator).GetHashCode(); } }
                
            public void Declare(IInstantiatorBinder binder)
            {
                binder.BindGadgetTo<IOwner>();
                binder.InjectGadgetTo<FrameRecorder>();
                binder.AddGadgetAs<FrameRecorder>();
            }

            public void Bind(IGadgetBinder binder)
            {
                binder.Bind(new FrameRecorder());
            }
            #endregion
        }

        public new interface IOwner
            : GadgetTools.IOwner
        {
            void GetDefaultFrame(out IRecordedFrame defaultFrame);
        }

        public interface IRecorder
            : IGadget
        {
        }

        public interface IRecordedFrame
        {
            #region Properties
            IRecorder SourceRecorder { get; }
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
}