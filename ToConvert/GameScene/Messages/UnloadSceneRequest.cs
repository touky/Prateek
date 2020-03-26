namespace Assets.Prateek.ToConvert.GameScene.Messages
{
    using Assets.Prateek.ToConvert.Messaging.Messages;

    public class UnloadSceneRequest<TResponseType> : RequestMessage<TResponseType>
        where TResponseType : UnloadSceneResponse, new()
    {
        #region Properties
        public string Scene { get; set; } = string.Empty;
        #endregion
    }
}
