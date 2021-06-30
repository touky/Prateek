namespace Prateek.Runtime.FrameRecorder
{
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class FrameRecorderInstantiator
        : IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        public int DefaultPriority { get { return typeof(FrameRecorderInstantiator).GetHashCode(); } }
                
        public void Declare(IInstantiatorBinder binder)
        {
            binder.BindTo<IFrameRecorderOwner>();
            binder.InjectGadgetTo<FrameRecorder>();
            binder.AddGadgetAs<FrameRecorder>();
        }

        public void Bind(IGadgetBinder binder)
        {
            binder.Bind(new FrameRecorder());
        }
        #endregion
    }
}
