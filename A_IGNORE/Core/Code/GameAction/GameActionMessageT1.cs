namespace Mayfair.Core.Code.GameAction
{
    using Prateek.KeynameFramework;
    using Prateek.KeynameFramework.Interfaces;
    using Prateek.CommandFramework;
    using Prateek.CommandFramework.TransmitterReceiver;

    public class GameActionCommand<T> : GameActionCommand
        where T : MasterKeyword
    {
        #region Constructors
        public GameActionCommand()
        {
            //tags.Add(Keyname.Create<T>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(ICommandEmitter transmitter, Keyname id0, float targetValue = 1)
        {
            GameActionCommand<T> command = Create<GameActionCommand<T>>();
            command.targetValue = targetValue;
            command.Add(id0);
            CommandDaemon.DefaultCommandEmitter.Broadcast(command);
        }

        public static void Broadcast(ICommandEmitter transmitter, Keyname id0, Keyname id1, float targetValue = 1)
        {
            GameActionCommand<T> command = Create<GameActionCommand<T>>();
            command.targetValue = targetValue;
            command.Add(id0);
            command.Add(id1);
            CommandDaemon.DefaultCommandEmitter.Broadcast(command);
        }
        #endregion
    }
}
