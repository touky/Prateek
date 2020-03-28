namespace Mayfair.Core.Code.GameScene.Messages
{
    using Messaging.Messages;

    public class UnloadSceneRequest<TResponseType> : RequestMessage<TResponseType>
        where TResponseType : UnloadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}