namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.NoticeFramework.Notices.Core;
    using Resources.Loader;

    public class LoadSceneResponse : ResponseNotice
    {
        public SceneReference SceneReference { get; set; }
    }
}