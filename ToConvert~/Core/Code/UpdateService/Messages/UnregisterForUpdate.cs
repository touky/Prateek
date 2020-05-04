namespace Mayfair.Core.Code.UpdateService.Messages
{
    using Interfaces;
    using Prateek.NoticeFramework.Notices.Core;

    public class UnregisterForUpdate : DirectNotice
    {
        public IUpdatable UpdatableObject { get; private set; }

        public void Init(IUpdatable updatableObject)
        {
            UpdatableObject = updatableObject;
        }
    }
}