// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using UnityEditor;

    ///todo [InitializeOnLoad]
    class MixedFuncScriptActionLoader : PrateekScriptBuilder
    {
        static MixedFuncScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            MixedFuncScriptAction.Create(Tag.importExtension).Commit();
        }
    }
}