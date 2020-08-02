namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using Prateek.Editor.CodeGeneration.CodeBuilder.ScriptTemplates;
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using Prateek.Runtime.Core.Helpers;
    using UnityEditor;

    [InitializeOnLoad]
    internal class ScriptActionSyntaxLoader
    {
        #region Constructors
        static ScriptActionSyntaxLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            ScriptFileTemplate.Create(Glossary.importExtension.Extension(Glossary.exportExtension), Glossary.exportExtension)
                .SetAutorun(false)
                .SetTemplateFile(string.Empty)
                .Load("InternalContent_Prateek_script.txt")
                .Commit();
        }
        #endregion
    }
}
