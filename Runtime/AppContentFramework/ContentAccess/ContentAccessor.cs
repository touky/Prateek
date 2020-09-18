namespace Prateek.Runtime.AppContentFramework.Daemons
{
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
        private IContentAccessorOwner owner;
        #endregion

        #region Constructors
        public ContentAccessor(IContentAccessorOwner owner)
        {
            this.owner = owner;
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
            var receiver = owner.Get<ICommandReceiver>();
            Assert.IsNotNull(receiver);

            receiver.SetActionFor(responseAction);
            receiver.ApplyActionChanges();

            var request = CommandHelper.Create<TRequest, TResponse>();
            request.Setup(contentAccessSettings);
            receiver.Send(request);
        }
        #endregion

        #region IGadget Members
        public void Kill()
        {
            //todo: unregister
        }
        #endregion
    }
}
