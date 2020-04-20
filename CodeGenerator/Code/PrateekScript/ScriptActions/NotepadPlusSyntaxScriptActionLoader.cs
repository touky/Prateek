// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using UnityEditor;

    ///todo [InitializeOnLoad]
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
