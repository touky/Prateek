namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Commands;
    using Prateek.Runtime.AppContentFramework.Unity.Handles;

    public abstract class GameObjectResourceServant : VisualResourceServant<GameObjectHandle>
    {
        #region Unity Methods
        public override void Startup()
        {
            base.Startup();

            Init();
        }
        #endregion
        
        #region Class Methods
        public void OnResourceChanged(VisualResourceDaemonOverseer daemonOverseerCore, ContentAccessChangedResponse notice)
        {
            if (notice is GameObjectContentAccessResponse typedMessage)
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

        protected void OnResourceChanged(GameObjectContentAccessResponse notice)
        {
            //todo DebugTools.Log(this, notice);

            //for (int r = 0; r < notice.References.Count; r++)
            //{
            //    GameObjectContentHandle resource = notice.References[r];

            //    Store(resource);
            //}
        }

        //todo public override RequestAccessToContent GetResourceChangeRequest(ICommandEmitter transmitter)
        //todo {
        //todo     RequestAccessToContent request = CreateResourceChangeRequest();
        //todo     request.Init(ResourceKeywords);
        //todo     return request;
        //todo }
        //todo 
        //todo protected abstract RequestAccessToContent CreateResourceChangeRequest();
        protected abstract bool IsResponseAccepted(GameObjectContentAccessResponse response);

        protected abstract void Init();
        #endregion
    }
}
