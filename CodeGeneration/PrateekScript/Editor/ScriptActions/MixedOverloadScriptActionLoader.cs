// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using UnityEditor;

    [InitializeOnLoad]
    internal class MixedOverloadScriptActionLoader : PrateekScriptBuilder
    {
        #region Constructors
        static MixedOverloadScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            MixedOverloadScriptAction.Create(Glossary.importExtension).Commit();
        }
        #endregion
    }
}
