namespace Mayfair.Core.Code.GameData
{
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.GameData.Messages;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.SaveGame;
    using Mayfair.Core.Code.SaveGame.Messages;
    using Mayfair.Core.Code.Service;

    public abstract class GameDataServiceProvider : ServiceProviderBehaviour
    {
        #region Fields
        private ILightMessageCommunicator communicator;
        #endregion

        #region Properties
        protected ILightMessageCommunicator Communicator
        {
            get { return this.communicator; }
        }
        #endregion

        #region Class Methods
        public void Init(ILightMessageCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public virtual bool HasRequestForNotice(GameLoadingNotice message) { return false; }
        public virtual void FillSaveRequest(GameLoadingNotice message, LoadDataRequest request) { }

        public abstract bool ReceiveIdentificationData(SaveDataIdentification data);
        public virtual void OnGameDataLoaded(GameDataLoaded message) { }
        #endregion

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        public virtual void OnSessionDebugAvailable(SessionDebugAvailable message) { }
        [Conditional("NVIZZIO_DEV")]
        public virtual void OnDebugDraw(GameDataPage page, DebugMenuContext context) { }
        #endregion
    }
}
