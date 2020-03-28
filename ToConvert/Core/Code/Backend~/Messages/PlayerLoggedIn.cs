namespace Mayfair.Core.Code.Backend.Messages
{
    using Messaging.Messages;

    public class PlayerLoggedIn : BroadcastMessage
    {
        public string PlayerId { get; private set; }
        public bool IsNewPlayer { get; private set; }

        public void Init(string playerId, bool isNewPlayer)
        {
            PlayerId = playerId;
            IsNewPlayer = isNewPlayer;
        }
    }
}