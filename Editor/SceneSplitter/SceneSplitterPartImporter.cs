namespace Prateek.Editor.SceneSplitter
{
    using UnityEditor.Experimental.AssetImporters;

    [ScriptedImporter(1, SceneSplitter.scenePartExtension)]
    public class SceneSplitterPartImporter : ScriptedImporter
    {
        #region Class Methods
        public override void OnImportAsset(AssetImportContext ctx) { }
        #endregion
    }
}
