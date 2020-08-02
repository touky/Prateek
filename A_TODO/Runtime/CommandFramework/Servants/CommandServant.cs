namespace Prateek.A_TODO.Runtime.CommandFramework.Servants
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.DaemonFramework.Servants;

    public abstract class CommandServant : ServantTickable<CommandDaemon, CommandServant>
    {
        #region Class Methods
        public abstract void ReceiveNotice(CommandDaemon daemonCore, Command receivedCommand);
        #endregion
    }
}
