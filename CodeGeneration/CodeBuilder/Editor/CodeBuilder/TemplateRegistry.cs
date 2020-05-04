namespace Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder
{
    using System.Collections.Generic;
    using Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates;
    using Prateek.EditorJobSystem.Code;

    public static class TemplateRegistry
    {
        ///---------------------------------------------------------------------

        #region Scripts
        private static ConcurrentList<ScriptFileTemplate> scripts = new ConcurrentList<ScriptFileTemplate>();

        public static IReadOnlyList<ScriptFileTemplate> Scripts
        {
            get
            {
                EditorJobSystem.JoinWork();

                return scripts.Copy;
            }
        }

        public static void Add(ScriptFileTemplate data)
        {
            scripts.Add(data);
        }
        #endregion Scripts

        ///---------------------------------------------------------------------

        #region Keywords
        private static ConcurrentList<KeywordTemplate> keywords = new ConcurrentList<KeywordTemplate>();

        public static IReadOnlyList<KeywordTemplate> Keywords
        {
            get
            {
                EditorJobSystem.JoinWork();

                return keywords.Copy;
            }
        }

        public static void Add(KeywordTemplate data)
        {
            keywords.Add(data);
        }
        #endregion Keywords

        ///---------------------------------------------------------------------

        #region Ignorables
        private static ConcurrentList<IgnorableTemplate> ignorables = new ConcurrentList<IgnorableTemplate>();

        public static IReadOnlyList<IgnorableTemplate> Ignorables
        {
            get
            {
                EditorJobSystem.JoinWork();

                return ignorables.Copy;
            }
        }

        public static void Add(IgnorableTemplate data)
        {
            ignorables.Add(data);
        }
        #endregion Ignorables

        ///---------------------------------------------------------------------

        #region Unity templates
        private static ConcurrentList<UnityFileTemplate> templates = new ConcurrentList<UnityFileTemplate>();

        public static void Add(UnityFileTemplate data)
        {
            templates.Add(data);
        }

        public static bool MatchTemplate(string filePath, string extension, string content)
        {
            EditorJobSystem.JoinWork();

            var list = templates.Copy;
            for (int t = 0; t < list.Count; t++)
            {
                var template = list[t];
                if (template.FullName != filePath)
                    continue;

                return template.Match(template.FileName, extension, content);
            }

            return false;
        }
        #endregion Unity templates
    }
}
