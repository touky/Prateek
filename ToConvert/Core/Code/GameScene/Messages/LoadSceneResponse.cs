namespace Mayfair.Core.Code.GameScene.Messages
{
    using Messaging.Messages;
    using Resources.Loader;

    public class LoadSceneResponse : ResponseMessage
    {
        public SceneReference SceneReference { get; set; }
    }
}