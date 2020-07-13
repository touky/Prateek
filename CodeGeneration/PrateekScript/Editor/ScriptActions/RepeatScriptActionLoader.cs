// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.PrateekScript.Editor.ScriptActions
{
    using Prateek.CodeGeneration.Code.PrateekScript;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using UnityEditor;

    [InitializeOnLoad]
    internal class RepeatScriptActionLoader : PrateekScriptBuilder
    {
        #region Constructors
        static RepeatScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            RepeatScriptAction.Create(Glossary.importExtension).Commit();
        }
        #endregion
    }
}

