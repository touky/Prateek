namespace Prateek.Runtime.FrameRecorder
{
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class FrameRecorderInstantiator
        //: IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        ///-----------------------------------------------------------------
        public int DefaultPriority { get { return typeof(FrameRecorderInstantiator).GetHashCode(); } }

        ///-----------------------------------------------------------------
        public void Create(IGadgetOwner owner)
        {
            if (!(owner is IFrameRecorderOwner typedOwner))
            {
                return;
            }

            var recorder = new FrameRecorder(typedOwner);
            owner.GadgetPouch.Add(recorder);
            FrameRecorderRegistry.Register(recorder);
        }
        #endregion
    }
}
