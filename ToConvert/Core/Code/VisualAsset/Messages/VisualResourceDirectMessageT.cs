namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.VisualAsset.Providers;

    public abstract class VisualResourceDirectMessage<TDaemonBranch> : VisualResourceDirectMessage
        where TDaemonBranch : VisualResourceDaemonBranch
    {
        #region Class Methods
        public override bool AllowTransfer(VisualResourceDaemonBranch branch)
        {
            return branch.GetType() == typeof(TDaemonBranch);
        }
        #endregion
    }
}