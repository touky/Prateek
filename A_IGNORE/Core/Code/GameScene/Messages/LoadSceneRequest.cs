namespace Mayfair.Core.Code.GameScene.Messages
{
    using Commands.Core;

    public class LoadSceneRequest<TResponseType> : RequestCommand<TResponseType>
        where TResponseType : LoadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}