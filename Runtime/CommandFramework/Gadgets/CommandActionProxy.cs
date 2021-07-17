namespace Prateek.Runtime.CommandFramework.Gadgets
{
    using System.Diagnostics;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Gadgets.Interfaces;
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

        #region ICommandActionProxy Members
        public void Invoke(Command command)
        {
            var tCommand = command as T;
            Assert.IsNotNull(tCommand, $"typedMessage is null for: notice = {command.GetType().Name}, T = {typeof(T).Name}");
            if (action != null)
            {
                action.Invoke(tCommand);
            }
        }
        #endregion
    }
}
