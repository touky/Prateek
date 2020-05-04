// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using UnityEditor;

    [InitializeOnLoad]
    internal class MixedFuncScriptActionLoader : PrateekScriptBuilder
    {
        #region Constructors
        static MixedFuncScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            MixedFuncScriptAction.Create(Glossary.importExtension).Commit();
        }
        #endregion
    }
}
