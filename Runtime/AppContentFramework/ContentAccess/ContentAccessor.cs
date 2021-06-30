namespace Prateek.Runtime.AppContentFramework.ContentAccess
{
    using Prateek.Runtime.AppContentFramework.ContentAccess.Interfaces;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.EmitterReceiver;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using UnityEngine.Assertions;

    public class ContentAccessor
        : IGadget
    {
        #region Fields
        private ContentAccessRequest accessRequest;
        private IContentAccessorOwner Owner { get; set; }
        #endregion

        #region Constructors
        public ContentAccessor()
        {
        }
        #endregion

        #region Class Methods
        public void SendAccesRequest<TResponse>(CommandAction<TResponse> responseAction, ContentAccessSettings contentAccessSettings)
            where TResponse : ContentAccessChangedResponse, new()
        {
            SendAccesRequest<ContentAccessRequest, TResponse>(responseAction, contentAccessSettings);
        }

        public void SendAccesRequest<TRequest, TResponse>(CommandAction<TResponse> responseAction, ContentAccessSettings contentAccessSettings)
            where TRequest : ContentAccessRequest, new()
            where TResponse : ContentAccessChangedResponse, new()
        {
            var receiver = Owner.Get<ICommandReceiver>();
            Assert.IsNotNull(receiver);

            receiver.SetActionFor(responseAction);
            receiver.ApplyActionChanges();

            var request = CommandHelper.Create<TRequest, TResponse>();
            request.Setup(contentAccessSettings);
            receiver.Send(request);
        }
        #endregion

        #region IGadget Members
        public void Awake()
        {
            Owner.SetupContentAccess(this);
        }

        public void Kill()
        {
            //todo: unregister
        }
        #endregion
    }
}
