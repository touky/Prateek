namespace Assets.Prateek.ToConvert.GameScene.Messages
{
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.Resources.Loader;

    public class LoadSceneResponse : ResponseMessage
    {
        #region Properties
        public SceneReference SceneReference { get; set; }
        #endregion
    }
}
