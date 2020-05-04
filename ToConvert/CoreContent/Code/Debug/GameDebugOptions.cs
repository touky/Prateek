namespace Mayfair.CoreContent.Code.Debug
{
    public class GameDebugOptions
    {
        #region Properties
        public static GameDebugOptions Instance { get; } = new GameDebugOptions();

        public BuildingDebugSettings BuildingDebug { get; } = new BuildingDebugSettings();
        #endregion
    }
}
