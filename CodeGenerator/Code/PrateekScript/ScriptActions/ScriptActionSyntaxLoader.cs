namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using global::Prateek.CodeGenerator;
    using global::Prateek.CodeGenerator.ScriptTemplates;
    using global::Prateek.Core.Code.Helpers;
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
                .SetFileContent("InternalContent_Prateek_script.txt")
                .Commit();
        }
        #endregion
    }
}
