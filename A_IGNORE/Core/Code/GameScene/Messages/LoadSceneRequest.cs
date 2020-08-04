namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class LoadSceneRequest<TResponseType> : RequestCommand<TResponseType, TResponseType>
        where TResponseType : LoadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}