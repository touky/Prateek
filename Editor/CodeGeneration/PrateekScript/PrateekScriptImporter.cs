namespace Prateek.CodeGeneration.PrateekScript.Editor
{
    using System;
    using System.Collections.Generic;
    using Prateek.CodeGeneration.Code.PrateekScript;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
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

