namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public class UnloadSceneRequest<TResponseType> : RequestCommand
        where TResponseType : UnloadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;

        protected override bool ValidateResponse()
        {
            return holder.Validate<LoadSceneResponse>();
        }
    }
}