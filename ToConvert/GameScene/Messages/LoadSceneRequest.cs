namespace Assets.Prateek.ToConvert.GameScene.Messages
{
    using Assets.Prateek.ToConvert.Messaging.Messages;

    public class LoadSceneRequest<TResponseType> : RequestMessage<TResponseType>
        where TResponseType : LoadSceneResponse, new()
    {
        #region Properties
        public string Scene { get; set; } = string.Empty;
        #endregion
    }
}
