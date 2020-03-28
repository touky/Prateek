namespace Mayfair.Core.Code.UpdateService.Messages
{
    using Interfaces;
    using Messaging.Messages;

    public class RegisterForUpdate : DirectMessage
    {
        public IUpdatable UpdatableObject { get; private set; }
        public UpdateFrequency UpdateFrequency { get; private set; }

        public void Init(IUpdatable updatableObject, UpdateFrequency updateFrequency)
        {
            UpdatableObject = updatableObject;
            UpdateFrequency = updateFrequency;
        }
    }
}