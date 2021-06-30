namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.FrameRecorder;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class DebugMenuDocumentInstantiator
        : IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        public int DefaultPriority
        {
            get { return typeof(DebugMenuDocumentInstantiator).GetHashCode(); }
        }
                        
        public void Declare(IInstantiatorBinder binder)
        {
            binder.BindTo<IDebugMenuDocumentOwner>();
            //todo binder.InjectGadgetTo<DebugMenuDocument>();
            binder.AddGadgetAs<DebugMenuDocument>();
        }

        public void Bind(IGadgetBinder binder)
        {
            binder.Bind(new DebugMenuDocument());
        }
        #endregion
    }
}
