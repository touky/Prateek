// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using UnityEditor;

    [InitializeOnLoad]
    internal class OverloadFuncScriptActionLoader : PrateekScriptBuilder
    {
        #region Constructors
        static OverloadFuncScriptActionLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            OverloadFuncScriptAction.Create(Glossary.importExtension).Commit();
        }
        #endregion
    }
}
