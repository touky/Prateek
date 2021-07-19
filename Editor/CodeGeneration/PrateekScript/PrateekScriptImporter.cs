namespace Prateek.Editor.CodeGeneration.PrateekScript
{
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using UnityEditor.Experimental.AssetImporters;

    //todo fix [ScriptedImporter(1, Glossary.importExtension)]
    //todo fix public class PrateekScriptImporter : ScriptedImporter
    //todo fix {
    //todo fix     #region Class Methods
    //todo fix     public override void OnImportAsset(AssetImportContext ctx)
    //todo fix     {
    //todo fix         var builder = PrateekScriptBuilder.GetInstance();
    //todo fix         builder.AddFile(ctx.assetPath);
    //todo fix         PrateekScriptBuilderEditorWindow.AddBuilder(builder);
    //todo fix     }
    //todo fix     #endregion
    //todo fix }
}

