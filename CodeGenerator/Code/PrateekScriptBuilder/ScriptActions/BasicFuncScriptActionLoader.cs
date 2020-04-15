// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using UnityEditor;

    ///todo [InitializeOnLoad]
    class BasicFuncScriptActionLoader : PrateekScriptBuilder
    {
        static BasicFuncScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            BasicFuncScriptAction.Create(Tag.importExtension).Commit();
        }
    }
}