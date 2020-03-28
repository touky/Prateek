namespace Mayfair.Core.Code.UpdateService
{
    using System;
    using Service;
    using Types.Extensions;

    /// <summary>
    /// This should not be used as an example of Service/Provider/Messaging interaction
    /// as it violates the asynchronicity goals of our systems.
    /// </summary>
    public class UpdateProvider : ServiceProviderBehaviour
    {
        public Action updateAction;

        public override bool IsProviderValid
        {
            get => true;
        }

        public override int Priority
        {
            get => 0;
        }

        #region  Unity Methods

        private void Update()
        {
            updateAction.SafeInvoke();
        }

        #endregion

        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<UpdateService, UpdateProvider>(this);
        }
    }
}