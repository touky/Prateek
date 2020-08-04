namespace Prateek.A_TODO.Runtime.CommandFramework.Servants
{
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.DaemonFramework.Servants;

    public abstract class CommandServant : ServantTickable<CommandDaemon, CommandServant>
    {
        #region Fields
        protected List<Command> commandReceived = new List<Command>();
        #endregion

        #region Class Methods
        public abstract void CommandReceived(Command command);

        public void FlushReceivedCommands()
        {
            foreach (var command in commandReceived)
            {
                Overseer.FlushCommand(command);
            }
        }
        #endregion
    }
}
