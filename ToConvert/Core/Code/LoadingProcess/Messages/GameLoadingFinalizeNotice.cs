namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;

    /// <summary>
    /// Loading message to warn late loading systems to load themselves
    /// Fires after GameplayNotice
    /// </summary>
    public class GameLoadingFinalizeNotice : GameLoadingNotice { }
}
