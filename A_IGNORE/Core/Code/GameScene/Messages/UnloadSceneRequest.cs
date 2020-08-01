namespace Mayfair.Core.Code.GameScene.Messages
{
    using Commands.Core;

    public class UnloadSceneRequest<TResponseType> : RequestCommand<TResponseType>
        where TResponseType : UnloadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}