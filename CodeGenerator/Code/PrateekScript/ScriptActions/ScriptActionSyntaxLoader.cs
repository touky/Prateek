namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using Prateek.CodeGenerator.ScriptTemplates;
    using Prateek.Core.Code.Helpers;
    using UnityEditor;

    [InitializeOnLoad]
    internal class ScriptActionSyntaxLoader : ScriptTemplate
    {
        #region Constructors
        static ScriptActionSyntaxLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            NewScript(Glossary.importExtension.Extension(Glossary.exportExtension), Glossary.exportExtension)
                .SetAutorun(false)
                .SetTemplateFile(string.Empty)
                .SetFileContent("InternalContent_Prateek_script.txt")
                .Commit();
        }
        #endregion
    }
}
