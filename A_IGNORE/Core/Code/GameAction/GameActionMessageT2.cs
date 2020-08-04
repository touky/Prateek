namespace Mayfair.Core.Code.GameAction
{
    using Prateek.A_TODO.Runtime.CommandFramework;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Interfaces;

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
            CommandDaemon.DefaultEmitter.Send(command);
        }

        public static void Broadcast(ICommandEmitter transmitter, Keyname id0, float targetValue = 1)
        {
            GameActionCommand<T0, T1> command = Create<GameActionCommand<T0, T1>>();
            command.targetValue = targetValue;
            command.Add(id0);
            CommandDaemon.DefaultEmitter.Send(command);
        }
        #endregion
    }
}
