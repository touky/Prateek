// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using Prateek.Core.Code.Helpers;
    using UnityEditor;

    ///todo [InitializeOnLoad]
    class NotepadPlusSyntaxTemplateLoader : ScriptTemplates.ScriptTemplate
    {
        static NotepadPlusSyntaxTemplateLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NewScript(PrateekScriptBuilder.Tag.importExtension.Extension("xml"), "xml")
                .SetAutorun(false)
                .SetEndsWith("SyntaxAutoComplete")
                .SetTemplateFile(String.Empty)
                .SetFileContent("InternalContent_PrateekCodegenSyntaxAutoComplete.xml.txt")
                .Commit();

            NewScript(PrateekScriptBuilder.Tag.importExtension.Extension("xml"), "xml")
                .SetAutorun(false)
                .SetTemplateFile(String.Empty)
                .SetEndsWith("SyntaxColor")
                .SetFileContent("InternalContent_PrateekCodegenSyntaxColor.xml.txt")
                .Commit();
        }
    }
}