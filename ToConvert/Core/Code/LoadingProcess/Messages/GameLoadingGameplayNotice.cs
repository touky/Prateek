namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;

    /// <summary>
    /// Loading message to warn gameplay systems to load themselves
    /// Fires after RequisiteNotice
    /// </summary>
    public class GameLoadingGameplayNotice : GameLoadingNotice { }
}
