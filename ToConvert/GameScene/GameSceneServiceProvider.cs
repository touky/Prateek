namespace Assets.Prateek.ToConvert.GameScene
{
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.Resources;
    using Assets.Prateek.ToConvert.Resources.Messages;

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

        #region Class Methods
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<GameSceneService, GameSceneServiceProvider>(this);
        }

        public override RequestCallbackOnChange GetResourceChangeRequest(ILightMessageCommunicator communicator)
        {
            var request = Message.Create<RequestCallbackOnSceneChange<SceneResourceHasChanged>>();
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
            //DebugTools.Log(this, message);

            for (var r = 0; r < message.References.Count; r++)
            {
                var resource = message.References[r];

                service.Add(resource);
            }
        }
        #endregion
    }
}
