namespace Mayfair.Core.Code.BaseBehaviour
{
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonFramework.Code.Servants;

    public sealed class AutoDisableServant
        : ServantBehaviour<AutoDisableDaemon, AutoDisableServant>
    {
        #region Properties
        public override int Priority
        {
            get { return 0; }
        }
        #endregion
    }
}
