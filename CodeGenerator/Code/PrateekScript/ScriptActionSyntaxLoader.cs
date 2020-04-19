namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
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

            NewScript(PrateekScriptBuilder.Tag.importExtension.Extension(PrateekScriptBuilder.Tag.exportExtension), PrateekScriptBuilder.Tag.exportExtension)
                .SetAutorun(false)
                .SetTemplateFile(string.Empty)
                .SetFileContent("InternalContent_Prateek_script.txt")
                .Commit();
        }
        #endregion
    }
}
