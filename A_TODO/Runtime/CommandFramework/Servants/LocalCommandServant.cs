namespace Prateek.A_TODO.Runtime.CommandFramework.Servants
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public sealed class LocalCommandServant : CommandServant
    {
        #region Class Methods
        public override void CommandReceived(Command command)
        {
            commandReceived.Add(command);
        }
        #endregion
    }
}
