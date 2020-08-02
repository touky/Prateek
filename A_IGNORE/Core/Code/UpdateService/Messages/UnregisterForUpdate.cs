namespace Mayfair.Core.Code.UpdateService.Messages
{
    using Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class UnregisterForUpdate : DirectCommand
    {
        public IUpdatable UpdatableObject { get; private set; }

        public void Init(IUpdatable updatableObject)
        {
            UpdatableObject = updatableObject;
        }
    }
}