namespace Prateek.Runtime.AppContentFramework.ContentAccess.Gadgets
{
    using JetBrains.Annotations;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework.Gadgets;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public abstract class ContentAccess
        : GadgetTools
    {
        [UsedImplicitly]
        internal class Instantiator
            : IInstantiator
        {
            #region IGadgetInstantiator Members
            public int DefaultPriority
            {
                get { return CommandTools.InstantiatorHashCode() + Const.NEXT_ITEM; }
            }
        
            public void Declare(IInstantiatorBinder binder)
            {
                binder.BindTo<IAccessorOwner>();
                binder.InjectGadgetTo<ContentAccessor>();
                binder.AddGadgetAs<ContentAccessor>();
            }

            public void Bind(IGadgetBinder binder)
            {
                binder.Bind(new ContentAccessor());
            }
            #endregion
        }

        public interface IAccessorOwner
            : CommandTools.IReceiverOwner
        {
            #region Class Methods
            IAccessor ContentAccessor { get; }
            void SetupContentAccess(IAccessor contentAccessor);
            #endregion
        }

        public interface IAccessor
            : IGadget
        {
            #region Class Methods
            void SendAccesRequest<TResponse>(CommandAction<TResponse> responseAction, ContentAccessSettings contentAccessSettings)
                where TResponse : ContentAccessChangedResponse, new();

            void SendAccesRequest<TRequest, TResponse>(CommandAction<TResponse> responseAction, ContentAccessSettings contentAccessSettings)
                where TRequest : ContentAccessRequest, new()
                where TResponse : ContentAccessChangedResponse, new();
            #endregion
        }
    }
}