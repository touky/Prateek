namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.NoticeFramework.Notices.Core;

    public class UnloadSceneRequest<TResponseType> : RequestNotice<TResponseType>
        where TResponseType : UnloadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}