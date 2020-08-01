namespace Prateek.CommandFramework.Servants
{
    using Commands.Core;

    public sealed class LocalCommandServant : CommandServant
    {
        #region Class Methods
        public override void ReceiveNotice(CommandDaemon daemonCore, Command receivedCommand)
        {
            daemonCore.AddMessage(receivedCommand);
        }
        #endregion
    }
}
