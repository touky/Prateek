namespace Mayfair.Core.Editor.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;
    using Mayfair.Core.Code.Database.Interfaces;
    using Mayfair.Core.Code.Protocol;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Mayfair.Plugins.AtDbMayfair.Editor;

    public class ProtocolBuildCodeGenerator
    {
        #region Static and Constants
        private const string TAG_ALIGNEMENT = "#TAG_ALIGNEMENT#";

        private const string TAG_NAMESPACE_EMPTY = "#NAMESPACE_EMPTY#";
        private const string TAG_NAME_DB_HELPER = "#HELPER_CLASS_NAME#";
        private const string TAG_NAME_UNPACKER = "#UNPACKER_CLASS_NAME#";
        private const string TAG_NAME_INTEGRATOR = "#TAG_NAME_INTEGRATOR#";

        private const string TAG_NAMESPACE_USING = "#NAMESPACE_ID#";
        private const string TAG_UNPACK_TYPE = "#CLASS_ID#";
        private const string TAG_UNPACK_CALLS = "#UNPACK_CODE#";
        private const string TAG_CODE_CONTAINER = "#CONTAINER_ENTRY_CODE#";
        private const string TAG_CONTAINER_RETURN = "#CONTAINER_RETURN_TAG#";
        private const string TAG_CONTAINER_COMMENT = "#COMMENT_CODE#";

        private const string TAG_PREFIX_LO = "id";
        private const string TAG_PREFIX_UP = "Id";
        private const string TAG_CODE_DATANAME = "#DATA_NAME_CODE#";
        private const string TAG_CODE_DATA = "#DATA_ENTRY_CODE#";

        private const string TAG_PATCH_REGEX = "#TAG_REGEX#";
        #endregion

        #region Fields
        private string NAMESPACE_EMPTY;
        private string NAMESPACE_DATABASE;
        private string NAMESPACE_DIAGNOSTICS;
        private string NAMESPACE_UNIQUEID;
        private string NAMESPACE_DEBUG;
        private string NAME_UNIQUEID;
        private string NAME_DATABASEHELPER;
        private string NAME_UNPACKER;
        private string NAME_INTEGRATOR;
        private string NAME_INTEGRATION;
        private string NAME_ENVELOPE;
        private string NAME_PROTO_CONTAINER;
        private string NAME_FULL_CONTAINER;
        private string NAME_FULL_DATA;
        private string CODE_NAMESPACE_USING = string.Empty;
        private string CODE_NAMESPACE_OPEN = string.Empty;
        private string CODE_NAMESPACE_CLOSE = string.Empty;
        private string CODE_TRYUNPACK = string.Empty;
        private string CODE_CONTAINER_RETURN = string.Empty;
        private string CODE_CONTAINER_COMMENT = string.Empty;
        private string CODE_CONTAINER_CONTENT = string.Empty;
        private string CODE_ENTRY_CONTENT_BEGIN = string.Empty;
        private string CODE_ENTRY_CONTENT_FIELD = string.Empty;
        private string CODE_ENTRY_CONTENT_PROPERTY = string.Empty;
        private string CODE_ENTRY_CONTENT_END = string.Empty;
        private string CODE_HEADER = string.Empty;
        private string CODE_DECLARATIONS = string.Empty;
        private string CODE_UNPACK_CALLS = string.Empty;
        private string CODE_DATABASE_HELPER = string.Empty;
        private string CODE_INTEGRATOR = string.Empty;

        private string CODE_PATCH_REGEX = string.Empty;
        private string CODE_PATCH_OBSOLETE = string.Empty;

        private string endNamespace = string.Empty;
        private string helperClassName = string.Empty;
        private string unpackerClassName = string.Empty;
        private string integratorClassName = string.Empty;
        private string integrationClassName = string.Empty;
        private string lastNamespace = string.Empty;
        private string containerCode = string.Empty;
        private string namespaceUsingCode = string.Empty;
        private string entryCode = string.Empty;
        private string unpackCode = string.Empty;
        private Dictionary<string, string> commentsFound = new Dictionary<string, string>();

        private string[] files = new string[3];
        private string[] code = new string[3];
        #endregion

        #region Properties
        public string[] Files
        {
            get { return files; }
        }

        public string[] Code
        {
            get { return code; }
        }
        #endregion

        #region Class Methods
        public void Init(string contextName, string endNamespace, string containerEntry)
        {
            //This is temporary and too "simple", instead of if-else, this can be done with a Map<Type, delegate>
            //Mostly because if-else is not optimized at-all
            //TODO, mostly
            NAMESPACE_DATABASE = GetNamespace(typeof(DatabaseHelper));
            NAMESPACE_DIAGNOSTICS = GetNamespace(typeof(DebuggerDisplayAttribute));
            NAMESPACE_UNIQUEID = GetNamespace(typeof(UniqueId));
            NAMESPACE_DEBUG = GetNamespace(typeof(DebugTools));
            NAME_UNIQUEID = GetName(typeof(UniqueId));
            NAME_DATABASEHELPER = GetName(typeof(DatabaseHelper));
            NAME_UNPACKER = GetName(DatabaseHelper.UnpackerType);
            NAME_INTEGRATION = "DatabaseIntegration";
            NAME_INTEGRATOR = "Integrator";
            NAME_ENVELOPE = GetName(typeof(ProtocolDatabaseEnvelope));
            NAME_PROTO_CONTAINER = GetName(typeof(ProtocolDatabaseContainer));
            NAME_FULL_CONTAINER = GetFullName(typeof(IDatabaseTable));
            NAME_FULL_DATA = GetFullName(typeof(IDatabaseEntry));

            this.endNamespace = endNamespace;
            helperClassName = $"{contextName}{NAME_DATABASEHELPER}";
            unpackerClassName = $"{contextName}{NAME_UNPACKER}";
            integratorClassName = $"{contextName}{NAME_INTEGRATOR}";
            integrationClassName = $"{contextName}{NAME_INTEGRATION}";

            CODE_NAMESPACE_USING = $@"
    using {TAG_NAMESPACE_USING};
";
            CODE_NAMESPACE_OPEN = $@"
    namespace {TAG_NAMESPACE_USING}
    {{
";
            CODE_NAMESPACE_CLOSE = @"
    }
";

            CODE_CONTAINER_CONTENT = $@"
    public sealed partial class {TAG_UNPACK_TYPE} : {NAME_FULL_CONTAINER}
    {{
        #region IContainerEntry Members
        public bool GetEntries(List<{NAME_FULL_DATA}> result) {{ {TAG_CONTAINER_RETURN} }}{TAG_CONTAINER_COMMENT}
        #endregion
    }}
";
            CODE_CONTAINER_RETURN = $@"return {NAME_DATABASEHELPER}.Transfer({containerEntry}, result);";
            CODE_CONTAINER_COMMENT = $@"
        public static string {AtDbEditorConstants.EXPORT_USAGE_COMMENT} = 
$@""#COMMENT_CODE#"";";

            CODE_ENTRY_CONTENT_BEGIN = $@"    [DebuggerDisplay(""{{GetType().Name}}, UniqueId: {{IdUnique}}"")]
    public sealed partial class {TAG_UNPACK_TYPE} : {NAME_FULL_DATA}
    {{
        #region IDatabaseEntry Members
";

            CODE_ENTRY_CONTENT_FIELD = $@"        private UniqueId {TAG_PREFIX_LO}{TAG_CODE_DATANAME} = string.Empty;
";
            CODE_ENTRY_CONTENT_PROPERTY = $@"        public UniqueId {TAG_PREFIX_UP}{TAG_CODE_DATANAME} {{ get {{ return {TAG_PREFIX_LO}{TAG_CODE_DATANAME}.Type == UniqueIdType.None ? ({TAG_PREFIX_LO}{TAG_CODE_DATANAME} = {TAG_CODE_DATA}) : {TAG_PREFIX_LO}{TAG_CODE_DATANAME}; }} }}
";

            CODE_ENTRY_CONTENT_END = @"        #endregion IDatabaseEntry Members
    }

";

            CODE_TRYUNPACK = $@"
                if (TryUnpack<{TAG_UNPACK_TYPE}>(envelope, out entry)) {{ return entry; }}";

            CODE_HEADER =
                @"//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//DO NOT MODIFY: THIS CODE IS AUTO GENERATED
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
";
            CODE_DECLARATIONS = $@"
namespace {TAG_NAMESPACE_EMPTY}
{{
    using System.Collections.Generic;
    using {NAMESPACE_DATABASE};
    using {NAMESPACE_DIAGNOSTICS};
    using {NAMESPACE_UNIQUEID};

{TAG_CODE_CONTAINER}

{TAG_CODE_DATA}
}}
";

            CODE_DATABASE_HELPER = $@"
namespace {TAG_NAMESPACE_EMPTY}
{{
    using {NAMESPACE_DATABASE};
    using {NAMESPACE_DEBUG};
{TAG_NAMESPACE_USING}

    public class {TAG_NAME_DB_HELPER} : {NAME_DATABASEHELPER}
    {{
        #region Fields
        private {TAG_NAME_UNPACKER} unpacker;
        #endregion

        #region Constructors
        public {TAG_NAME_DB_HELPER}()
        {{
            this.unpacker = new {TAG_NAME_UNPACKER}();
        }}
        #endregion

        #region Nested type: {TAG_NAME_UNPACKER}
        protected class {TAG_NAME_UNPACKER} : {NAME_UNPACKER}
        {{
            #region Class Methods
            public override {NAME_FULL_CONTAINER} TryUnpack({NAME_PROTO_CONTAINER} container)
            {{
                {NAME_ENVELOPE} envelope = container.GetEnvelope();
                {NAME_FULL_CONTAINER} entry = null;
{TAG_UNPACK_CALLS}

                return null;
            }}
            #endregion
        }}
        #endregion
    }}
}}
";
            CODE_INTEGRATOR = $@"
namespace {TAG_NAMESPACE_EMPTY}
{{
    using UnityEngine;

    public class {TAG_NAME_INTEGRATOR} : MonoBehaviour
    {{
        #region Fields
        private {TAG_NAME_DB_HELPER} helper;
        #endregion

        #region Unity Methods
        private void Awake()
        {{
            this.helper = new {TAG_NAME_DB_HELPER}();
        }}
        #endregion
    }}
}}
";

            CODE_PATCH_REGEX = $@"( *)public( *)string( *){TAG_PATCH_REGEX}( *){{";
            CODE_PATCH_OBSOLETE = $@"{TAG_ALIGNEMENT}[System.Obsolete(""Do not use outside of the IDatabaseEntry context"")]
";
        }

        public void BuildComments(string srcDir, bool reset, Func<List<string>, bool> gatherFiles)
        {
            if (!reset)
            {
                List<string> importFiles = new List<string>();
                if (gatherFiles(importFiles))
                {
                    for (int f = 0; f < importFiles.Count; f++)
                    {
                        string path = importFiles[f];
                        string content = File.ReadAllText(path);

                        MatchCollection messageContent = RegexHelper.ProtocolMessage.Matches(content);
                        MatchCollection commentContent = RegexHelper.XmlComment.Matches(content);
                        int commentIndex = 0;
                        if (messageContent.Count > 0 && commentContent.Count > 0)
                        {
                            for (int m = 0; m < messageContent.Count; m++)
                            {
                                Match messageMatch = messageContent[m];
                                string message = messageMatch.Groups[messageMatch.Groups.Count - 1].Value;
                                while (commentIndex < commentContent.Count)
                                {
                                    Match commentMatch = commentContent[commentIndex];
                                    Group commentGroup = commentMatch.Groups[commentMatch.Groups.Count - 1];
                                    if (commentGroup.Index >= messageMatch.Index)
                                    {
                                        break;
                                    }

                                    string commentValue = commentGroup.Value;
                                    if (!commentsFound.ContainsKey(message))
                                    {
                                        commentsFound.Add(message, string.Empty);
                                    }

                                    if (commentsFound[message] != string.Empty)
                                    {
                                        commentsFound[message] += "\n";
                                    }

                                    commentsFound[message] = commentsFound[message] + commentValue;
                                    commentIndex++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AddCodeElement(Type type, Type entryType, List<string> dataEntries, bool reset)
        {
            string innerNamespace = type.Namespace;
            int innerIndex = innerNamespace.IndexOf(endNamespace);
            if (innerIndex != Consts.INDEX_NONE)
            {
                if (NAMESPACE_EMPTY == null)
                {
                    NAMESPACE_EMPTY = innerNamespace.Substring(0, innerIndex + endNamespace.Length);
                }

                innerNamespace = innerNamespace.Substring(innerIndex + endNamespace.Length);
            }

            if (innerNamespace.StartsWith("."))
            {
                innerNamespace = innerNamespace.Remove(0, 1);
            }

            //Open a new namespace if needed
            if (lastNamespace != innerNamespace)
            {
                if (lastNamespace != string.Empty)
                {
                    containerCode += CODE_NAMESPACE_CLOSE;
                }

                string code = CODE_NAMESPACE_OPEN.Replace(TAG_NAMESPACE_USING, innerNamespace);
                containerCode += code;
                entryCode += code;
                namespaceUsingCode += CODE_NAMESPACE_USING.Replace(TAG_NAMESPACE_USING, type.Namespace);

                lastNamespace = innerNamespace;
            }

            string returnCode = CODE_CONTAINER_RETURN;

            //Deactivate all this code is we only need a reset
            if (reset)
            {
                returnCode = "return false;";
            }
            else
            {
                unpackCode += CODE_TRYUNPACK
                    .Replace(TAG_UNPACK_TYPE, type.Name);
            }

            string commentCode = string.Empty;
            if (commentsFound.ContainsKey(type.Name))
            {
                commentCode = CODE_CONTAINER_COMMENT.Replace(TAG_CONTAINER_COMMENT, commentsFound[type.Name]);
            }

            containerCode += CODE_CONTAINER_CONTENT
                             .Replace(TAG_UNPACK_TYPE, type.Name)
                             .Replace(TAG_CONTAINER_RETURN, returnCode)
                             .Replace(TAG_CONTAINER_COMMENT, commentCode);

            entryCode += CODE_ENTRY_CONTENT_BEGIN.Replace(TAG_UNPACK_TYPE, entryType.Name);
            {
                AddCodeEntryCode(CODE_ENTRY_CONTENT_FIELD, reset, entryType, dataEntries);
                AddCodeEntryCode(CODE_ENTRY_CONTENT_PROPERTY, reset, entryType, dataEntries);
            }
            entryCode += CODE_ENTRY_CONTENT_END;
        }

        private void AddCodeEntryCode(string sourceCode, bool reset, Type entryType, List<string> dataEntries)
        {
            string emptyContent = "string.Empty";
            foreach (string entry in dataEntries)
            {
                string realEntry = entry;
                if (realEntry != NAME_UNIQUEID)
                {
                    realEntry = realEntry.Replace(NAME_UNIQUEID, string.Empty);
                }

                realEntry = realEntry.Replace(TAG_PREFIX_UP, string.Empty);

                entryCode += sourceCode
                             .Replace(TAG_UNPACK_TYPE, entryType.Name)
                             .Replace(TAG_CODE_DATANAME, realEntry)
                             .Replace(TAG_CODE_DATA, reset ? emptyContent : entry);
            }
        }

        public bool BuildCode(string optionExtension, string defaultExtension)
        {
            if (containerCode != string.Empty)
            {
                //Open a new namespace if needed
                if (lastNamespace != string.Empty)
                {
                    containerCode += CODE_NAMESPACE_CLOSE;
                    entryCode += CODE_NAMESPACE_CLOSE;
                }

                for (int i = 0; i < code.Length; i++)
                {
                    code[i] = CODE_HEADER;
                    if (i == 0)
                    {
                        files[i] = $"{integrationClassName}{optionExtension}";
                        code[i] += CODE_DECLARATIONS
                                   .Replace(TAG_CODE_CONTAINER, containerCode)
                                   .Replace(TAG_CODE_DATA, entryCode)
                                   .Replace(TAG_NAMESPACE_EMPTY, NAMESPACE_EMPTY);
                    }
                    else if (i == 1)
                    {
                        files[i] = $"{helperClassName}{optionExtension}";
                        code[i] += CODE_DATABASE_HELPER
                                   .Replace(TAG_NAME_DB_HELPER, helperClassName)
                                   .Replace(TAG_NAME_UNPACKER, unpackerClassName)
                                   .Replace(TAG_UNPACK_CALLS, unpackCode)
                                   .Replace(TAG_NAMESPACE_USING, namespaceUsingCode)
                                   .Replace(TAG_NAMESPACE_EMPTY, NAMESPACE_EMPTY);
                    }
                    else if (i == 2)
                    {
                        //Integrator needs the default .cs extension because MonoBehaviour & Unity
                        files[i] = $"{integratorClassName}{defaultExtension}";
                        code[i] += CODE_INTEGRATOR
                                   .Replace(TAG_NAME_INTEGRATOR, integratorClassName)
                                   .Replace(TAG_NAME_DB_HELPER, helperClassName)
                                   .Replace(TAG_UNPACK_CALLS, unpackCode)
                                   .Replace(TAG_NAMESPACE_USING, namespaceUsingCode)
                                   .Replace(TAG_NAMESPACE_EMPTY, NAMESPACE_EMPTY);
                    }
                }

                return true;
            }

            return false;
        }

        public bool PatchCodeWithObsolete(ref string content, List<string> patchEntries)
        {
            bool appliedPatch = false;
            int index = 0;
            for (int c = 0; c < patchEntries.Count; c++)
            {
                string propertyName = patchEntries[c];
                Regex searchRegex = new Regex(CODE_PATCH_REGEX.Replace(TAG_PATCH_REGEX, propertyName));
                while (true)
                {
                    Match match = searchRegex.Match(content, index);
                    if (!match.Success)
                    {
                        break;
                    }

                    appliedPatch = true;
                    index = match.Index;

                    Group group = match.Groups[1];
                    string patchCode = CODE_PATCH_OBSOLETE.Replace(TAG_ALIGNEMENT, group.Value);
                    content = content.Insert(index, patchCode);
                    index += match.Length + patchCode.Length;
                }
            }

            return appliedPatch;
        }
        #endregion

        #region Name helpers
        public static string GetName(Type type)
        {
            return type.Name;
        }

        public static string GetFullName(Type type)
        {
            return type.FullName.Replace('+', '.');
        }

        public static string GetNamespace(Type type)
        {
            return type.Namespace;
        }
        #endregion
    }
}
