namespace Mayfair.Core.Code.UpdateService.Messages
{
    using Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class RegisterForUpdate : DirectCommand
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