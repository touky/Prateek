// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
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
