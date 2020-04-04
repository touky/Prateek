namespace Mayfair.Core.Code.GameScene.Messages
{
    using Prateek.NoticeFramework.Notices.Core;

    public class LoadSceneRequest<TResponseType> : RequestNotice<TResponseType>
        where TResponseType : LoadSceneResponse, new()
    {
        public string Scene { get; set; } = string.Empty;
    }
}