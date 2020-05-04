namespace Mayfair.Core.Code.BaseBehaviour
{
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonCore.Code.Branches;

    public sealed class AutoDisableDaemonBranch
        : DaemonBranchBehaviour<AutoDisableDaemonCore, AutoDisableDaemonBranch>
    {
        #region Properties
        public override int Priority
        {
            get { return 0; }
        }
        #endregion
    }
}
