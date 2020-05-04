// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using UnityEditor;

    [InitializeOnLoad]
    internal class NotepadPlusSyntaxScriptActionLoader : PrateekScriptBuilder
    {
        #region Constructors
        static NotepadPlusSyntaxScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            NotepadPlusSyntaxColorScriptAction.Create(Glossary.importExtension).Commit();
            NotepadPlusSyntaxAutoCompleteScriptAction.Create(Glossary.importExtension).Commit();
        }
        #endregion
    }
}
