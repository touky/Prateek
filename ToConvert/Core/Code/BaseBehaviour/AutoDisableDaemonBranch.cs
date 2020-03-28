namespace Mayfair.Core.Code.BaseBehaviour
{
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonCore.Code.Branches;

    public sealed class AutoDisableDaemonBranch : DaemonBranchBehaviour<AutoDisableDaemonCore, AutoDisableDaemonBranch>
    {
        #region Properties
        protected override bool IsAliveInternal
        {
            get { return true; }
        }

        public override int Priority
        {
            get { return 0; }
        }
        #endregion
    }
}
