namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using System;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public interface ICommandReceiver : ICommandEmitter
    {
        #region Class Methods
        //Sending
        void Send(TargetedCommand command);
        void Kill();
        /// <summary>
        /// This needs to be called to flush and process all received commands
        /// </summary>
        void ProcessReceivedCommands();
        #endregion

        #region Setup
        void SetActionForReception(Action onCommandReceived);
        void SetActionFor<T>(CommandAction<T> action) where T : Command;
        void ClearActionFor<T>() where T : Command;
        void ClearAllActions();
        void ApplyActionChanges();
        #endregion
    }
}

