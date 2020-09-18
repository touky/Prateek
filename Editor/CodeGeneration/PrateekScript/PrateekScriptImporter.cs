namespace Prateek.Editor.CodeGeneration.PrateekScript
{
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using UnityEditor.Experimental.AssetImporters;

    [ScriptedImporter(1, Glossary.importExtension)]
    public class PrateekScriptImporter : ScriptedImporter
    {
        #region Class Methods
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var builder = PrateekScriptBuilder.GetInstance();
            builder.AddFile(ctx.assetPath);
            PrateekScriptBuilderEditorWindow.AddBuilder(builder);
        }
        #endregion
    }
}

