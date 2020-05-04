namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public abstract class GameObjectResourceDaemonBranch : VisualResourceDaemonBranch<GameObjectContentHandle>
    {
        #region Unity Methods
        public override void Startup()
        {
            base.Startup();

            Init();
        }
        #endregion
        
        #region Class Methods
        public override void OnResourceChanged(VisualResourceDaemonCore daemonCore, ResourcesHaveChangedResponse notice)
        {
            if (notice is GameObjectResourcesHaveChanged typedMessage)
            {
                if (IsResponseAccepted(typedMessage))
                {
                    OnResourceChanged(typedMessage);
                }
            }
        }

        public override void OnVisualResourceMessage(VisualResourceDirectNotice notice)
        {
            if (notice.Instance != null)
            {
                AddPendingInit(notice.Instance.AssignmentIndex, notice.Instance);
            }
            else
            {
                DebugTools.LogError($"notice.Instance is null in OnVisualResourceMessage for {name}: Message: {notice.GetType().Name}");
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

        public override RequestAccessToContent GetResourceChangeRequest(INoticeTransmitter transmitter)
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
