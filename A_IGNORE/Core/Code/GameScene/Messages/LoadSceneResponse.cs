namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.Runtime.AppContentFramework.Unity.Handles;
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public class LoadSceneResponse : ResponseCommand
    {
        public SceneHandle SceneHandle { get; set; }
    }
}