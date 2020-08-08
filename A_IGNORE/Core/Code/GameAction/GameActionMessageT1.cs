namespace Mayfair.Core.Code.GameAction
{
    using Prateek.A_TODO.Runtime.CommandFramework;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Interfaces;

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
            GameActionCommand<T> command = CommandHelper.Create<GameActionCommand<T>>();
            command.targetValue = targetValue;
            command.Add(id0);
            CommandDaemon.DefaultEmitter.Send(command);
        }

        public static void Broadcast(ICommandEmitter transmitter, Keyname id0, Keyname id1, float targetValue = 1)
        {
            GameActionCommand<T> command = CommandHelper.Create<GameActionCommand<T>>();
            command.targetValue = targetValue;
            command.Add(id0);
            command.Add(id1);
            CommandDaemon.DefaultEmitter.Send(command);
        }
        #endregion
    }
}
