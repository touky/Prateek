namespace Mayfair.Core.Code.UpdateService.Messages
{
    using Interfaces;
    using Messaging.Messages;

    public class UnregisterForUpdate : DirectMessage
    {
        public IUpdatable UpdatableObject { get; private set; }

        public void Init(IUpdatable updatableObject)
        {
            UpdatableObject = updatableObject;
        }
    }
}