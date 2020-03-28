namespace Mayfair.Core.Editor.Protocol
{
    using Mayfair.Core.Editor.EditorWindows;
    using UnityEditor.Experimental.AssetImporters;

    [ScriptedImporter(1, "proto")]
    public class ProtocolImporter : ScriptedImporter
    {
        #region Class Methods
        public override void OnImportAsset(AssetImportContext ctx)
        {
            BaseProcessEditorWindow.DoOpenWithAutoExecute<ProtocolBuildEditorWindow>(ctx.assetPath);
        }
        #endregion
    }
}
