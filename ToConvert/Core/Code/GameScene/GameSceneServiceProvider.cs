namespace Mayfair.Core.Code.GameScene
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Resources;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Helpers;
    using UnityEngine;

    public sealed class GameSceneServiceProvider : ResourceDependentServiceProvider<GameSceneService, GameSceneServiceProvider>
    {
        #region Static and Constants
        public static readonly string[] KEYWORDS = {"Scenes/"};
        #endregion

        #region Properties
        public override string[] ResourceKeywords
        {
            get { return KEYWORDS; }
        }

        public override bool IsProviderValid
        {
            get { return true; }
        }

        public override int Priority
        {
            get { return 0; }
        }
        #endregion

        #region Service
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<GameSceneService, GameSceneServiceProvider>(this);
        }
        #endregion

        #region Class Methods
        public override RequestCallbackOnChange GetResourceChangeRequest(ILightMessageCommunicator communicator)
        {
            RequestCallbackOnSceneChange<SceneResourceHasChanged> request = Message.Create<RequestCallbackOnSceneChange<SceneResourceHasChanged>>();
            request.Init(ResourceKeywords);
            return request;
        }

        public override void OnResourceChanged(GameSceneService service, ResourcesHaveChangedResponse message)
        {
            if (message is SceneResourceHasChanged)
            {
                OnSceneResourceChanged(service, (SceneResourceHasChanged) message);
            }
        }

        private void OnSceneResourceChanged(GameSceneService service, SceneResourceHasChanged message)
        {
            DebugTools.Log(this, message);

            for (int r = 0; r < message.References.Count; r++)
            {
                SceneReference resource = message.References[r];

                service.Add(resource);
            }
        }
        #endregion
    }
}
