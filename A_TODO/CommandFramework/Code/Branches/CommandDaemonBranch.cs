namespace Prateek.CommandFramework.Servants
{
    using Commands.Core;
    using Prateek.DaemonFramework.Code.Servants;

    public abstract class CommandServant : ServantBehaviour<CommandDaemon, CommandServant>
    {
        #region Class Methods
        public abstract void ReceiveNotice(CommandDaemon daemonCore, Command receivedCommand);
        #endregion
    }
}
