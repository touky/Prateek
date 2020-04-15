// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using UnityEditor;

    ///todo [InitializeOnLoad]
    class SwizzleScriptActionLoader : PrateekScriptBuilder
    {
        static SwizzleScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            SwizzleScriptAction.Create(Tag.importExtension).Commit();
        }
    }
}