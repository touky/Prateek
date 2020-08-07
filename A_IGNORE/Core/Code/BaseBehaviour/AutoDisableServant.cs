namespace Mayfair.Core.Code.BaseBehaviour
{
    using Mayfair.Core.Code.Service;
    using Prateek.Runtime.DaemonFramework.Servants;

    public sealed class AutoDisableServant
        : Servant<AutoDisableDaemon, AutoDisableServant>
    {
        #region Properties
        public override int Priority
        {
            get { return 0; }
        }
        #endregion
    }
}
