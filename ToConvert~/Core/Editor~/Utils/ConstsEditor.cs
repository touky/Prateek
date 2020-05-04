namespace Mayfair.Core.Editor.Utils
{
    public static class ConstsEditor
    {
        #region Static and Constants
        public const string ERROR = "ERROR";

        public const string ANIM_SEPARATOR = "@";

        public const string LOD = "lod";

        public const string UNIQUE = "unique";
        public const string PREFAB = "prefab";
        public const string ASSET = "asset";
        public const string ANIM = "anim";
        public const string ANIMATIONS = "Animations";
        public const string FBX = "FBX";
        public const string FOCUS = "Focus";
        public const string CLONE = "(Clone)";

        public const string MISSING = "MISSING";
        public const string FOUND = "FOUND";
        public const string OUT_OF_DATE = "OUT OF DATE";

        public const string DUMMY = "Dummy";
        public const string BASE_PLATE = "BasePlate";

        public const int INDENT_BACKUP = 0;
        public const int ONE_INDENT = 1;
        public const int TWO_INDENT = 2;
        #endregion

        #region Class Methods
        public static string RemoveCloneTag(this string text)
        {
            return text.Replace(CLONE, string.Empty);
        }
        #endregion
    }
}
