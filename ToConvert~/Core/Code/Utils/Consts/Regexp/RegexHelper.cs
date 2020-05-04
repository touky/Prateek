namespace Mayfair.Core.Code.Utils.Helpers.Regexp
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class RegexHelper
    {
        #region Properties
        /// <summary>
        ///     Matches "Thing_mesh_tree00001" into { Thing, mesh, tree00001 }
        /// </summary>
        public static Regex UniqueIdTag
        {
            get { return RegexContent.UNIQUEID_TAG_REGEX; }
        }

        /// <summary>
        ///     Matches "Dir_Other01 @thing10" into { " @" }
        /// </summary>
        public static Regex NonWord
        {
            get { return RegexContent.NON_WORD_REGEX; }
        }

        /// <summary>
        ///     Matches "test_nothing 1" into { " 1" }
        /// </summary>
        public static Regex UnityImportRegex
        {
            get { return RegexContent.UNITY_IMPORT_REGEX; }
        }

        /// <summary>
        ///     Matches "Dir/Other/thing" into { Dir, Other }
        /// </summary>
        public static Regex FolderSplit
        {
            get { return RegexContent.FOLDER_SPLIT_REGEX; }
        }

        /// <summary>
        ///     Matches "Thing_mesh_LOD0" into { LOD0 }
        /// </summary>
        public static Regex LodDetect
        {
            get { return RegexContent.LOD_DETECT_REGEX; }
        }

        /// <summary>
        ///     Matches "Thing_mesh_tree00001" into { Thing, mesh, tree }
        /// </summary>
        public static Regex AssetTag
        {
            get { return RegexContent.ASSET_TAG_REGEX; }
        }

        /// <summary>
        ///     Matches "/Thing_mesh_tree00001" into { Thing, mesh, tree }
        /// </summary>
        public static Regex AddressTag
        {
            get { return RegexContent.ADDRESS_TAG_REGEX; }
        }

        /// <summary>
        ///     Matches "AvenueConnecticutDeed" into { Avenue, Connecticut, Deed }
        /// </summary>
        public static Regex NameDetect
        {
            get { return RegexContent.NAME_DETECT_REGEX; }
        }

        /// <summary>
        ///     Matches "AvenueConnecticutDeed" into { Avenue, Connecticut, Deed }
        /// </summary>
        public static Regex PrefixDetect
        {
            get { return RegexContent.PREFIX_DETECT_REGEX; }
        }

        /// <summary>
        ///     Matches "1_Property_1Start_Avenue" into { 1_ }
        /// </summary>
        public static Regex NumberStart
        {
            get { return RegexContent.NUMBER_START_REGEX; }
        }

        /// <summary>
        ///     Matches "1_Property_1Start_Avenue" into { Property, Avenue }
        /// </summary>
        public static Regex NameSurrounded
        {
            get { return RegexContent.NAME_SURROUNDED_REGEX; }
        }

        /// <summary>
        ///     Matches "1_Property_1Start_Avenue" into { P, S, A }
        /// </summary>
        public static Regex UpperCase
        {
            get { return RegexContent.UPPERCASE_REGEX; }
        }

        /// <summary>
        ///     Matches "///This is a comment" into { This is a comment }
        /// </summary>
        public static Regex XmlComment
        {
            get { return RegexContent.XML_COMMENT_REGEX; }
        }

        /// <summary>
        ///     Matches "notice MyThing" into { MyThing }
        /// </summary>
        public static Regex ProtocolMessage
        {
            get { return RegexContent.PROTOCOL_MESSAGE_REGEX; }
        }
        #endregion

        #region Class Methods
        public static bool TryFetchingMatches(string source, Regex regex, List<string> matches)
        {
            List<Match> list = new List<Match>();
            if (TryFetchingMatches(source, regex, list))
            {
                foreach (Match match in list)
                {
                    matches.Add(match.Groups[match.Groups.Count - 1].Value);
                }

                return true;
            }

            return false;
        }

        public static bool TryFetchingMatches(string source, Regex regex, List<Match> matches)
        {
            if (source == null)
            {
                return false;
            }

            Match match = regex.Match(source);
            while (match != null && match.Success)
            {
                matches.Add(match);

                match = match.NextMatch();
            }

            return matches.Count != 0;
        }
        #endregion
    }
}
