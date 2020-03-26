namespace Assets.Prateek.ToConvert.GameScene
{
    using Assets.Prateek.ToConvert.Messaging.Messages;

    internal class GameSessionOpen : BroadcastMessage
    {
        public string SessionContext;
    }
}