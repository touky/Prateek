namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class ContentAccessorInstantiator
        : IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        public int DefaultPriority
        {
            get { return typeof(CommandReceiverInstantiator).GetHashCode() + Const.NEXT_ITEM; }
        }

        public void Create(IGadgetOwner owner)
        {
            if (!(owner is IContentAccessorOwner contentUser))
            {
                return;
            }

            var contentAccess = new ContentAccessor(contentUser);
            owner.GadgetPouch.Add(contentAccess);
            contentUser.SetupContentAccess(contentAccess);
        }
        #endregion
    }
}
