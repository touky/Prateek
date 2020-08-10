namespace Mayfair.Core.Code.Database.ServiceProvider.DatabaseIdentification
{
    using Mayfair.Core.Code.BaseBehaviour;
    using Mayfair.Core.Code.Utils;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    public abstract class DatabaseIdentificationIntegrator : CommandReceiverOwner
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
        public override void DefineReceptionActions(ICommandReceiver receiver)  { }

        public abstract void Create();
        #endregion
    }
}
