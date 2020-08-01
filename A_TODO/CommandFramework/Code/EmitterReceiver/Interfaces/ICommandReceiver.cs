namespace Prateek.CommandFramework.TransmitterReceiver
{
    using System;
    using Commands.Core;

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

