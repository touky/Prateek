namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver
{
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using UnityEngine.Assertions;

    [DebuggerDisplay("Callback: {action.Method.Name}, Type: {GetType().Name}")]
    public class CommandActionProxy<T> : ICommandActionProxy
        where T : Command
    {
        #region Fields
        private CommandAction<T> action;
        #endregion

        #region Constructors
        public CommandActionProxy(CommandAction<T> action)
        {
            this.action = action;
        }
        #endregion

        #region IMessageCallbackProxy Members
        public void Invoke(Command command)
        {
            T tCommand = command as T;
            Assert.IsNotNull(tCommand, $"typedMessage is null for: notice = {command.GetType().Name}, T = {typeof(T).Name}");
            if (this.action != null)
            {
                this.action.Invoke(tCommand);
            }
        }
        #endregion
    }
}
