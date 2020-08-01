namespace Mayfair.Core.Code.GameScene
{
    using Mayfair.Core.Code.Resources;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Helpers;
    using Commands.Core;
    using Prateek.CommandFramework.TransmitterReceiver;
    using UnityEngine;

    public sealed class GameSceneServant : ContentAccessServant<GameSceneDaemon, GameSceneServant>
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
        public override RequestAccessToContent GetResourceChangeRequest(ICommandEmitter transmitter)
        {
            RequestCallbackOnSceneChange<SceneResourceHasChanged> request = Command.Create<RequestCallbackOnSceneChange<SceneResourceHasChanged>>();
            request.Init(ResourceKeywords);
            return request;
        }

        public override void OnResourceChanged(GameSceneDaemon daemonCore, ResourcesHaveChangedResponse notice)
        {
            if (notice is SceneResourceHasChanged)
            {
                OnSceneResourceChanged(daemonCore, (SceneResourceHasChanged) notice);
            }
        }

        private void OnSceneResourceChanged(GameSceneDaemon daemonCore, SceneResourceHasChanged notice)
        {
            //todo DebugTools.Log(this, notice);

            for (int r = 0; r < notice.References.Count; r++)
            {
                SceneReference resource = notice.References[r];

                daemonCore.Add(resource);
            }
        }
        #endregion
    }
}
