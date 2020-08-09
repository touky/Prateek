namespace Mayfair.Core.Code.VisualAsset.Providers
{
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Commands;
    using Prateek.Runtime.AppContentFramework.Unity.Handles;
    using UnityEngine;

    public abstract class ScriptableObjectResourceServant<TScriptableResourceType>
        : VisualResourceServant<ScriptableObjectHandle<TScriptableResourceType>>
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
        public void OnResourceChanged(VisualResourceDaemonOverseer daemonOverseerCore, ContentAccessChangedResponse notice)
        {
            if (notice is ScriptableContentAccessChangedResponse<TScriptableResourceType> typedMessage)
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

        protected void OnResourceChanged(ScriptableContentAccessChangedResponse<TScriptableResourceType> notice)
        {
            //todo DebugTools.Log(this, notice);

            //for (int r = 0; r < notice.References.Count; r++)
            //{
            //    ScriptableObjectContentHandle<TScriptableResourceType> resource = notice.References[r];

            //    Store(resource);
            //}
        }
        
        //todo public override RequestAccessToContent GetResourceChangeRequest(ICommandEmitter transmitter)
        //todo {
        //todo     RequestAccessToContent request = CreateResourceChangeRequest();
        //todo     request.Init(ResourceKeywords);
        //todo     return request;
        //todo }

        //todo protected abstract RequestAccessToContent CreateResourceChangeRequest();
        protected abstract bool IsResponseAccepted(ScriptableContentAccessChangedResponse<TScriptableResourceType> response);

        protected abstract void Init();
        #endregion
    }
}
