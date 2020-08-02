namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration;
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public abstract class GameObjectResourceServant : VisualResourceServant<GameObjectContentHandle>
    {
        #region Unity Methods
        public override void Startup()
        {
            base.Startup();

            Init();
        }
        #endregion
        
        #region Class Methods
        public override void OnResourceChanged(VisualResourceDaemon daemonCore, ResourcesHaveChangedResponse notice)
        {
            if (notice is GameObjectResourcesHaveChanged typedMessage)
            {
                if (IsResponseAccepted(typedMessage))
                {
                    OnResourceChanged(typedMessage);
                }
            }
        }

        public override void OnVisualResourceMessage(VisualResourceDirectCommand command)
        {
            if (command.Instance != null)
            {
                AddPendingInit(command.Instance.AssignmentIndex, command.Instance);
            }
            else
            {
                DebugTools.LogError($"notice.Instance is null in OnVisualResourceMessage for {Name}: Message: {command.GetType().Name}");
            }
        }

        protected void OnResourceChanged(GameObjectResourcesHaveChanged notice)
        {
            //todo DebugTools.Log(this, notice);

            for (int r = 0; r < notice.References.Count; r++)
            {
                GameObjectContentHandle resource = notice.References[r];

                Store(resource);
            }
        }

        public override RequestAccessToContent GetResourceChangeRequest(ICommandEmitter transmitter)
        {
            RequestAccessToContent request = CreateResourceChangeRequest();
            request.Init(ResourceKeywords);
            return request;
        }

        protected abstract RequestAccessToContent CreateResourceChangeRequest();
        protected abstract bool IsResponseAccepted(GameObjectResourcesHaveChanged response);

        protected abstract void Init();
        #endregion
    }
}
