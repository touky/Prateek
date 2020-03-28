using Mayfair.Core.Code.Messaging;
using Mayfair.Core.Code.Messaging.Communicator;


namespace Mayfair.Core.Code.GameAction
{
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Types.UniqueId;

    public class GameActionMessage<T0, T1> : GameActionMessage
        where T0 : MasterTag
        where T1 : MasterTag
    {
        #region Constructors
        public GameActionMessage()
        {
            tags.Add(Keyname.Create<T0>());
            tags.Add(Keyname.Create<T1>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(ILightMessageCommunicator communicator, float targetValue = 1)
        {
            GameActionMessage<T0, T1> message = Create<GameActionMessage<T0, T1>>();
            message.targetValue = targetValue;
            MessageService.DefaultCommunicator.Broadcast(message);
        }

        public static void Broadcast(ILightMessageCommunicator communicator, Keyname id0, float targetValue = 1)
        {
            GameActionMessage<T0, T1> message = Create<GameActionMessage<T0, T1>>();
            message.targetValue = targetValue;
            message.Add(id0);
            MessageService.DefaultCommunicator.Broadcast(message);
        }
        #endregion
    }
}
