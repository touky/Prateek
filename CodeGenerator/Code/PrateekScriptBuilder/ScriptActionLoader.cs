namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using Prateek.Core.Code.Helpers;
    using UnityEditor;

    ///todo
    [InitializeOnLoad]
    class ScriptActionLoader : Prateek.CodeGenerator.ScriptTemplates.ScriptTemplate
    {
        static ScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewScript(PrateekScriptBuilder.Tag.importExtension.Extension(PrateekScriptBuilder.Tag.exportExtension), PrateekScriptBuilder.Tag.exportExtension)
                .SetAutorun(false)
                .SetTemplateFile(String.Empty)
                .SetFileContent("InternalContent_Prateek_script.txt")
                .Commit();
        }
    }
}