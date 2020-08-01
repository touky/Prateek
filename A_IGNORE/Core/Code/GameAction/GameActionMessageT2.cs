namespace Mayfair.Core.Code.GameAction
{
    using Prateek.KeynameFramework;
    using Prateek.KeynameFramework.Interfaces;
    using Prateek.CommandFramework;
    using Prateek.CommandFramework.TransmitterReceiver;

    public class GameActionCommand<T0, T1> : GameActionCommand
        where T0 : MasterKeyword
        where T1 : MasterKeyword
    {
        #region Constructors
        public GameActionCommand()
        {
            //tags.Add(Keyname.Create<T0>());
            //tags.Add(Keyname.Create<T1>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(ICommandEmitter transmitter, float targetValue = 1)
        {
            GameActionCommand<T0, T1> command = Create<GameActionCommand<T0, T1>>();
            command.targetValue = targetValue;
            CommandDaemon.DefaultCommandEmitter.Broadcast(command);
        }

        public static void Broadcast(ICommandEmitter transmitter, Keyname id0, float targetValue = 1)
        {
            GameActionCommand<T0, T1> command = Create<GameActionCommand<T0, T1>>();
            command.targetValue = targetValue;
            command.Add(id0);
            CommandDaemon.DefaultCommandEmitter.Broadcast(command);
        }
        #endregion
    }
}
