// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using UnityEditor;

    [InitializeOnLoad]
    class MixedCTorScriptActionLoader : PrateekScriptBuilder
    {
        static MixedCTorScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            MixedCTorScriptAction.Create(Tag.importExtension).Commit();
        }
    }
}