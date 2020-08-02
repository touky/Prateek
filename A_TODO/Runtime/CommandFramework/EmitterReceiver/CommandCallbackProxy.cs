namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver
{
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using UnityEngine.Assertions;

    public delegate void CommandCallback<T>(T notice) where T : Command;

    [DebuggerDisplay("Callback: {callback.Method.Name}, Type: {GetType().Name}")]
    public class CommandCallbackProxy<T> : ICommandCallbackProxy
        where T : Command
    {
        #region Fields
        private CommandCallback<T> callback;
        #endregion

        #region Constructors
        public CommandCallbackProxy(CommandCallback<T> callback)
        {
            this.callback = callback;
        }
        #endregion

        #region IMessageCallbackProxy Members
        public void Invoke(Command command)
        {
            T typedNotice = command as T;
            Assert.IsNotNull(typedNotice, $"typedMessage is null for: notice = {command.GetType().Name}, T = {typeof(T).Name}");
            if (typedNotice != null && this.callback != null)
            {
                this.callback.Invoke(typedNotice);
            }
        }
        #endregion
    }
}
