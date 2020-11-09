namespace Prateek.Runtime.Core.Consts
{
    public static class ConstFolder
    {
        #region Static and Constants
        public const string PROJECT_SETTINGS = "ProjectSettings";
        public const string PROJECT = "Project";
        public const string SETTINGS = "Settings";
        public const string PRATEEK = "Prateek";
        public const string EDITOR = "Editor";
        public const string ASSETS = "Assets";
        public const string RESOURCES = "Resources";
        public const string SCRIPTS = "Scripts";
        public const string SCENES = "Scenes";

        public static readonly string ASSETS_RESOURCES = $"{ASSETS}/{PRATEEK}/{RESOURCES}";
        public const string PRATEEK_SETTINGS = SETTINGS;
        public static readonly string PRATEEK_EDITOR_SETTINGS = $"{ASSETS}/{PRATEEK}/{EDITOR}";
        #endregion
    }
}
