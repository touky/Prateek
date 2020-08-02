namespace Mayfair.Core.Code.GameAction
{
    using Prateek.A_TODO.Runtime.CommandFramework;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Interfaces;

    public class GameActionCommand<T0, T1, T2> : GameActionCommand
        where T0 : MasterKeyword
        where T1 : MasterKeyword
        where T2 : MasterKeyword
    {
        #region Constructors
        public GameActionCommand()
        {
            //tags.Add(Keyname.Create<T0>());
            //tags.Add(Keyname.Create<T1>());
            //tags.Add(Keyname.Create<T2>());
        }
        #endregion

        #region Class Methods
        public static void Broadcast(ICommandEmitter transmitter, float targetValue = 1)
        {
            GameActionCommand<T0, T1, T2> command = Create<GameActionCommand<T0, T1, T2>>();
            command.targetValue = targetValue;
            CommandDaemon.DefaultCommandEmitter.Broadcast(command);
        }

        public static void Broadcast(ICommandEmitter transmitter, Keyname uniqueId1, float targetValue = 1)
        {
            GameActionCommand<T0, T1, T2> command = Create<GameActionCommand<T0, T1, T2>>();
            command.targetValue = targetValue;
            CommandDaemon.DefaultCommandEmitter.Broadcast(command);
        }

        #endregion
    }
}
