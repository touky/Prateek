namespace Mayfair.Core.Code.GameScene.Messages
{
    using Commands.Core;
    using Resources.Loader;

    public class LoadSceneResponse : ResponseCommand
    {
        public SceneReference SceneReference { get; set; }
    }
}