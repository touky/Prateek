namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using UnityEngine;

    public abstract class ScriptableObjectResourceDaemonBranch<TScriptableResourceType> : VisualResourceDaemonBranch<ScriptableObjectResourceReference<TScriptableResourceType>>
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
        public override void OnResourceChanged(VisualResourceDaemonCore daemonCore, ResourcesHaveChangedResponse message)
        {
            if (message is ScriptableResourcesHaveChanged<TScriptableResourceType> typedMessage)
            {
                if (IsResponseAccepted(typedMessage))
                {
                    OnResourceChanged(typedMessage);
                }
            }
        }

        public override void OnVisualResourceMessage(VisualResourceDirectMessage message)
        {
            AddPendingInit(message.Instance.AssignmentIndex, message.Instance);
        }

        protected void OnResourceChanged(ScriptableResourcesHaveChanged<TScriptableResourceType> message)
        {
            //todo DebugTools.Log(this, message);

            for (int r = 0; r < message.References.Count; r++)
            {
                ScriptableObjectResourceReference<TScriptableResourceType> resource = message.References[r];

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
        protected abstract bool IsResponseAccepted(ScriptableResourcesHaveChanged<TScriptableResourceType> response);

        protected abstract void Init();
        #endregion
    }
}
