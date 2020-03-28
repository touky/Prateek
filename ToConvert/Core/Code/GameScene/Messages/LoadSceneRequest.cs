namespace Mayfair.Core.Code.GameScene.Messages
{
    using Messaging.Messages;

    public class LoadSceneRequest<TResponseType> : RequestMessage<TResponseType>
        where TResponseType : LoadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}