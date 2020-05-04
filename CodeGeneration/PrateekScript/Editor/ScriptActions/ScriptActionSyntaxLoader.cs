namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using UnityEditor;
    using Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using Prateek.Core.Code.Helpers;

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
