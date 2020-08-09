namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public class LoadSceneRequest<TResponseType> : RequestCommand
        where TResponseType : LoadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;

        protected override bool ValidateResponse()
        {
            return holder.Validate<LoadSceneResponse>();
        }
    }
}