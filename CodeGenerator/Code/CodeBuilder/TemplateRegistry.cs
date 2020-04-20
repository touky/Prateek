namespace Prateek.CodeGenerator {
    using System.Collections.Generic;
    using Prateek.CodeGenerator.ScriptTemplates;

    public static class TemplateRegistry
    {
        //---------------------------------------------------------------------
        #region Scripts
        private static List<ScriptFileTemplate> scripts = new List<ScriptFileTemplate>();
        public static TemplateGroup<ScriptFileTemplate> Scripts { get { return new TemplateGroup<ScriptFileTemplate>(scripts); } }
        public static void Add(ScriptFileTemplate data) { scripts.Add(data); }
        #endregion Scripts

        //---------------------------------------------------------------------
        #region Keywords
        private static List<KeywordTemplate> keywords = new List<KeywordTemplate>();
        public static TemplateGroup<KeywordTemplate> Keywords { get { return new TemplateGroup<KeywordTemplate>(keywords); } }
        public static void Add(KeywordTemplate data) { keywords.Add(data); }
        #endregion Keywords

        //---------------------------------------------------------------------
        #region Ignorables
        private static List<IgnorableTemplate> ignorables = new List<IgnorableTemplate>();
        public static TemplateGroup<IgnorableTemplate> Ignorables { get { return new TemplateGroup<IgnorableTemplate>(ignorables); } }
        public static void Add(IgnorableTemplate data) { ignorables.Add(data); }
        #endregion Ignorables

        //---------------------------------------------------------------------
        #region Unity templates
        private static List<UnityFileTemplate> templates = new List<UnityFileTemplate>();
        public static void Add(UnityFileTemplate data) { templates.Add(data); }
        public static bool MatchTemplate(string filePath, string extension, string content)
        {
            for (int t = 0; t < templates.Count; t++)
            {
                var template = templates[t];
                if (template.FullName != filePath)
                    continue;

                return template.Match(template.FileName, extension, content);
            }
            return false;
        }
        #endregion Unity templates
    }
}