namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class LoadSceneResponse : ResponseCommand
    {
        public SceneReference SceneReference { get; set; }
    }
}