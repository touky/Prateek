namespace Prateek.Runtime.AppContentFramework.ContentAccess
{
    using Prateek.Runtime.AppContentFramework.ContentAccess.Interfaces;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class ContentAccessorInstantiator
        : IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        public int DefaultPriority
        {
            get { return typeof(CommandReceiverInstantiator).GetHashCode() + Const.NEXT_ITEM; }
        }
        
        public void Declare(IInstantiatorBinder binder)
        {
            binder.BindTo<IContentAccessorOwner>();
            binder.InjectGadgetTo<ContentAccessor>();
            binder.AddGadgetAs<ContentAccessor>();
        }

        public void Bind(IGadgetBinder binder)
        {
            binder.Bind(new ContentAccessor());
        }
        #endregion
    }
}
