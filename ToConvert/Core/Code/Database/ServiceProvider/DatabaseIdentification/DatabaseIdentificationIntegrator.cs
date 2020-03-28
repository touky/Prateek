namespace Mayfair.Core.Code.Database.ServiceProvider.DatabaseIdentification
{
    using Mayfair.Core.Code.BaseBehaviour;
    using Mayfair.Core.Code.Utils;

    public abstract class DatabaseIdentificationIntegrator : CommunicatorBehaviour
    {
        #region Fields
        private int destroyTicker = Consts.WAIT_5_FRAMES;
        #endregion

        #region Unity Methods
        protected void Start()
        {
            Create();
        }

        protected override void Update()
        {
            base.Update();

            if (this.destroyTicker-- < 0)
            {
                gameObject.SetActive(false);
            }
        }
        #endregion

        #region Class Methods
        protected override void SetupCommunicatorCallback() { }
        public override void MessageReceived() { }

        public abstract void Create();
        #endregion
    }
}
