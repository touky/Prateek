namespace Mayfair.Core.Editor.Protocol
{
    using System.IO;
    using Mayfair.Core.Code.Database;
    using Mayfair.Core.Code.Protocol;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Editor.Addressables;
    using UnityEditor.Experimental.AssetImporters;

    [ScriptedImporter(1, "protodb")]
    public class ProtocolDatabaseImporter : ScriptedImporter
    {
        #region Class Methods
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string fileName = Path.GetFileNameWithoutExtension(ctx.assetPath);
            byte[] bytes = File.ReadAllBytes(ctx.assetPath);
            ProtocolDatabaseContainer database = ProtocolDatabaseContainer.CreateInstance(bytes);
            if (database == null)
            {
                return;
            }

            ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), database);
            ctx.SetMainObject(database);

            string filename = Path.GetFileNameWithoutExtension(ctx.assetPath);
            string dirname = Path.GetDirectoryName(ctx.assetPath);
            string[] splits = dirname.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            dirname = splits.Length > 0 && splits[splits.Length - 1] != "Export" ? splits[splits.Length - 1] : string.Empty;

            string address = filename;
            if (dirname != string.Empty)
            {
                address = $"{dirname}/{address}";
            }

            string keyword = DatabaseServiceProvider.KEYWORDS[Consts.FIRST_ITEM];
            address = $"{keyword}{address}";
            string group = AddressableHelper.GetGroupFromName(filename, keyword.Remove(keyword.Length - 1));
            AddressableHelper.SetAddress(ctx.assetPath, address, group);
        }
        #endregion
    }
}
