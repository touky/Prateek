// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using UnityEditor;

    [InitializeOnLoad]
    internal class MixedCTorScriptActionLoader : PrateekScriptBuilder
    {
        #region Constructors
        static MixedCTorScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            MixedConstructorScriptAction.Create(Glossary.importExtension).Commit();
        }
        #endregion
    }
}

