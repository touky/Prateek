// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using UnityEditor;

    ///todo [InitializeOnLoad]
    class NotepadPlusSyntaxScriptActionLoader : PrateekScriptBuilder
    {
        static NotepadPlusSyntaxScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            NotepadPlusSyntaxColorScriptAction.Create(Tag.importExtension).Commit();
            NotepadPlusSyntaxAutoCompleteScriptAction.Create(Tag.importExtension).Commit();
        }
    }
}