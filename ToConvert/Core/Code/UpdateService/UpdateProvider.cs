namespace Mayfair.Core.Code.UpdateService
{
    using System;
    using Prateek.DaemonCore.Code.Branches;
    using Service;
    using Types.Extensions;

    /// <summary>
    /// This should not be used as an example of Service/Provider/Messaging interaction
    /// as it violates the asynchronicity goals of our systems.
    /// </summary>
    public class UpdateProvider : DaemonBranchBehaviour<UpdateDaemonCore, UpdateProvider>
    {
        public Action updateAction;

        protected override bool IsAliveInternal
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
    }
}