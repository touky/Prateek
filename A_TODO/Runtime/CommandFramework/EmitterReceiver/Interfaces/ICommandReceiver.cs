namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using System;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public interface ICommandReceiver : ICommandEmitter
    {
        #region Class Methods
        //Sending
        void Send(TargetedCommand command);
        void CleanUp();
        /// <summary>
        /// This needs to be called to flush and process all received commands
        /// </summary>
        void ProcessAllCommands();
        #endregion

        #region Setup
        void SetCommandReceived(Action onCommandReceived);
        void AddCallback<T>(CommandCallback<T> callback) where T : Command;
        void RemoveCallback<T>() where T : Command;
        void ClearCallbacks();
        void ApplyCallbacks();
        #endregion
    }
}

