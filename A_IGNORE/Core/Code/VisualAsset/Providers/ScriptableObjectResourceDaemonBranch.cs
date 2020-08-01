namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Prateek.CommandFramework.TransmitterReceiver;
    using UnityEngine;

    public abstract class ScriptableObjectResourceServant<TScriptableResourceType>
        : VisualResourceServant<ScriptableObjectContentHandle<TScriptableResourceType>>
        where TScriptableResourceType : ScriptableObject
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
            if (notice is ScriptableResourcesHaveChanged<TScriptableResourceType> typedMessage)
            {
                if (IsResponseAccepted(typedMessage))
                {
                    OnResourceChanged(typedMessage);
                }
            }
        }

        public override void OnVisualResourceMessage(VisualResourceDirectCommand command)
        {
            AddPendingInit(command.Instance.AssignmentIndex, command.Instance);
        }

        protected void OnResourceChanged(ScriptableResourcesHaveChanged<TScriptableResourceType> notice)
        {
            //todo DebugTools.Log(this, notice);

            for (int r = 0; r < notice.References.Count; r++)
            {
                ScriptableObjectContentHandle<TScriptableResourceType> resource = notice.References[r];

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
        protected abstract bool IsResponseAccepted(ScriptableResourcesHaveChanged<TScriptableResourceType> response);

        protected abstract void Init();
        #endregion
    }
}
