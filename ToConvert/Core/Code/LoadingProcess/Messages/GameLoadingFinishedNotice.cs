namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;

    /// <summary>
    /// Loading message to warn late loading systems have loaded, 
    /// fires after FinalizeNotice
    /// </summary>
    public class GameLoadingFinishedNotice : GameLoadingNotice { }
}