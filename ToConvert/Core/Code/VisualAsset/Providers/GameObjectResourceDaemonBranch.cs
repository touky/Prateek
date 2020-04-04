namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.VisualAsset.Messages;

    public abstract class GameObjectResourceDaemonBranch : VisualResourceDaemonBranch<GameObjectResourceReference>
    {
        #region Unity Methods
        public override void Startup()
        {
            base.Startup();

            Init();
        }
        #endregion
        
        #region Class Methods
        public override void OnResourceChanged(VisualResourceDaemonCore daemonCore, ResourcesHaveChangedResponse message)
        {
            if (message is GameObjectResourcesHaveChanged typedMessage)
            {
                if (IsResponseAccepted(typedMessage))
                {
                    OnResourceChanged(typedMessage);
                }
            }
        }

        public override void OnVisualResourceMessage(VisualResourceDirectMessage message)
        {
            if (message.Instance != null)
            {
                AddPendingInit(message.Instance.AssignmentIndex, message.Instance);
            }
            else
            {
                DebugTools.LogError($"message.Instance is null in OnVisualResourceMessage for {name}: Message: {message.GetType().Name}");
            }
        }

        protected void OnResourceChanged(GameObjectResourcesHaveChanged message)
        {
            DebugTools.Log(this, message);

            for (int r = 0; r < message.References.Count; r++)
            {
                GameObjectResourceReference resource = message.References[r];

                Store(resource);
            }
        }

        public override RequestCallbackOnChange GetResourceChangeRequest(ILightMessageCommunicator communicator)
        {
            RequestCallbackOnChange request = CreateResourceChangeRequest();
            request.Init(ResourceKeywords);
            return request;
        }

        protected abstract RequestCallbackOnChange CreateResourceChangeRequest();
        protected abstract bool IsResponseAccepted(GameObjectResourcesHaveChanged response);

        protected abstract void Init();
        #endregion
    }
}
