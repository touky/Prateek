namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class UnloadSceneRequest<TResponseType> : RequestCommand<TResponseType, TResponseType>
        where TResponseType : UnloadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}