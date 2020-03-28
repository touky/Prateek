namespace Mayfair.Core.Code.GameAction
{
    using Mayfair.Core.Code.Messaging;
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Types.UniqueId;

    public class GameActionMessage<T> : GameActionMessage
        where T : MasterTag
    {
        #region Constructors
        public GameActionMessage()
        {
            tags.Add(Keyname.Create<T>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(ILightMessageCommunicator communicator, Keyname id0, float targetValue = 1)
        {
            GameActionMessage<T> message = Create<GameActionMessage<T>>();
            message.targetValue = targetValue;
            message.Add(id0);
            MessageService.DefaultCommunicator.Broadcast(message);
        }

        public static void Broadcast(ILightMessageCommunicator communicator, Keyname id0, Keyname id1, float targetValue = 1)
        {
            GameActionMessage<T> message = Create<GameActionMessage<T>>();
            message.targetValue = targetValue;
            message.Add(id0);
            message.Add(id1);
            MessageService.DefaultCommunicator.Broadcast(message);
        }
        #endregion
    }
}
