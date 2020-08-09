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
        public override string[] ResourceKeywords
        {
            get { return KEYWORDS; }
        }
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

        protected override ContentAccessRequest CreateContentAccessRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnContentAccessChangedResponse(ContentAccessChangedResponse response)
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
