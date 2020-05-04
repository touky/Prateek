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
    public class UpdateProvider : DaemonBranchTickableBehaviour<UpdateDaemonCore, UpdateProvider>
    {
        public Action updateAction;

        #region  Unity Methods

        private void Update()
        {
            updateAction.SafeInvoke();
        }

        #endregion
    }
}