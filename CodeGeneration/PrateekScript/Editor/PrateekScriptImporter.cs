namespace Prateek.Core.Editor.AssetLibrary
{
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using UnityEditor.Experimental.AssetImporters;

    [ScriptedImporter(1, Glossary.importExtension)]
    public class PrateekScriptImporter : ScriptedImporter
    {
        #region Class Methods
        public override void OnImportAsset(AssetImportContext ctx)
        {
            //BaseProcessEditorWindow.DoOpenWithAutoExecute<ProtocolBuildEditorWindow>(ctx.assetPath);
        }
        #endregion
    }
}
