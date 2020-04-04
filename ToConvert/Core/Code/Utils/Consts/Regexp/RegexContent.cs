namespace Mayfair.Core.Code.Utils.Helpers.Regexp
{
    using System.Text.RegularExpressions;

    internal static class RegexContent
    {
        #region Static and Constants
        internal static string REPLACEMENT = "#REPLACE#";
        internal static readonly Regex UNIQUEID_TAG_REGEX = new Regex("(?:^|_)([A-Za-z][A-Za-z0-9]+)+");
        internal static readonly Regex NON_WORD_REGEX = new Regex(@"\W+");
        internal static readonly Regex UNITY_IMPORT_REGEX = new Regex(@"( [0-9]+)$");
        internal static readonly Regex FOLDER_SPLIT_REGEX = new Regex("([^\\/]+)(?:\\/)+");
        internal static readonly Regex ASSET_TAG_REGEX = new Regex("(?:^|_|0-9)([A-Za-z]+)+");
        internal static readonly Regex ADDRESS_TAG_REGEX = new Regex("(?:\\/|_|0-9)([A-Za-z]+)+");
        internal static readonly Regex LOD_DETECT_REGEX = new Regex("^([a-zA-Z0-9_]+)_(?:[Ll][Oo][Dd])([0-9]+)");
        internal static readonly Regex PREFIX_DETECT_REGEX = new Regex("^([A-Za-z]+)_");
        internal static readonly Regex NAME_DETECT_REGEX = new Regex("([A-Z][a-z]+)");
        internal static readonly Regex NUMBER_START_REGEX = new Regex("^([0-9]+_*)");
        internal static readonly Regex NAME_SURROUNDED_REGEX = new Regex("_*([A-Za-z]+)_*");
        internal static readonly Regex UPPERCASE_REGEX = new Regex("([A-Z]+)");
        internal static readonly Regex XML_COMMENT_REGEX = new Regex("\\/\\/\\/([a-zA-Z0-9_\\- .:;,?!(){}'\"]*)(?:[$\r\n])"); //(?:[^\r\n])
        internal static readonly Regex PROTOCOL_MESSAGE_REGEX = new Regex("(?:[^\r\n])\\s*notice\\s*([a-zA-Z0-9_]*)");
        internal static readonly string FOLDER_ROOT = $"\\/{REPLACEMENT}\\/([a-zA-Z]+)";
        #endregion
    }
}

