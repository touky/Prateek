namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Commands.Core;

    /// <summary>
    ///     Send this notice to request a reload from the game loading process
    /// </summary>
    public class GameLoadingNeedRestart : DirectCommand { }
}
