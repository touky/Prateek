namespace Mayfair.Core.Code.GameScene
{
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Helpers;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.AppContentFramework.Messages;
    using UnityEngine;

    public sealed class GameSceneServant : ContentAccessServant<GameSceneDaemonOverseer, GameSceneServant>
    {
        #region Static and Constants
        public static readonly string[] KEYWORDS = {"Scenes/"};
        #endregion

        #region Properties
        public string[] ResourceKeywords
        {
            get { return KEYWORDS; }
        }

        public string[] ResourceExtensions => throw new System.NotImplementedException();
        #endregion

        #region Class Methods
        //todo public override RequestAccessToContent GetResourceChangeRequest(ICommandEmitter transmitter)
        //todo {
        //todo     RequestCallbackOnSceneChange<SceneResourceHasChanged> request = Command.Create<RequestCallbackOnSceneChange<SceneResourceHasChanged>>();
        //todo     request.Init(ResourceKeywords);
        //todo     return request;
        //todo }

        public void OnResourceChanged(GameSceneDaemonOverseer daemonOverseerCore, ContentAccessChangedResponse notice)
        {
            if (notice is SceneResourceHasChangedResponse)
            {
                OnSceneResourceChanged(daemonOverseerCore, (SceneResourceHasChangedResponse) notice);
            }
        }

        public override void SetupContentAccess(ContentAccessor contentAccessor)
        {
            throw new System.NotImplementedException();
        }

        protected  ContentAccessRequest CreateContentAccessRequest()
        {
            throw new System.NotImplementedException();
        }

        protected  void OnContentAccessChangedResponse(ContentAccessChangedResponse response)
        {
            throw new System.NotImplementedException();
        }

        private void OnSceneResourceChanged(GameSceneDaemonOverseer daemonOverseerCore, SceneResourceHasChangedResponse notice)
        {
            //todo DebugTools.Log(this, notice);

            //for (int r = 0; r < notice.References.Count; r++)
            //{
            //    SceneReference resource = notice.References[r];

            //    daemonCore.Add(resource);
            //}
        }
        #endregion
    }
}
