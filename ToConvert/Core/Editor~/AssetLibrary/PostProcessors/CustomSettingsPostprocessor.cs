namespace Mayfair.Core.Editor.AssetLibrary.PostProcessors
{
    using JetBrains.Annotations;
    using Mayfair.Core.Editor.CustomImportSettings;
    using UnityEditor;

    public class CustomSettingsPostprocessor : AssetLibraryPostprocessor
    {
        #region Constructors
        [UsedImplicitly]
        public CustomSettingsPostprocessor() { }
        #endregion

        #region Class Methods
        public override uint GetVersion()
        {
            return 1 + base.GetVersion();
        }

        [UsedImplicitly]
        private void OnPreprocessModel()
        {
            if (ShouldIgnore(CustomImportType.CustomImportSettings, null, assetPath))
            {
                return;
            }

            FbxImportRules importRules = AssetLibrary.Get(CustomImportType.CustomImportSettings, assetPath);
            if (importRules == null || importRules.ImportSettings.fbx == null)
            {
                return;
            }

            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.importTangents = importRules.ImportSettings.fbx.importTangents;
        }

        [UsedImplicitly]
        private void OnPreprocessTexture()
        {
            if (ShouldIgnore(CustomImportType.CustomImportSettings, null, assetPath))
            {
                return;
            }

            FbxImportRules importRules = AssetLibrary.Get(CustomImportType.CustomImportSettings, assetPath);
            if (importRules == null || importRules.ImportSettings.texture == null)
            {
                return;
            }

            TextureImporter textureImporter = assetImporter as TextureImporter;
            textureImporter.streamingMipmaps = importRules.ImportSettings.texture.streamingMipmaps;
        }

        private void OnPreprocessAnimation()
        {
            if (ShouldIgnore(CustomImportType.CustomImportSettings, null, assetPath))
            {
                return;
            }

            FbxImportRules importRules = AssetLibrary.Get(CustomImportType.CustomImportSettings, assetPath);
            if (importRules == null || importRules.ImportSettings.animation == null)
            {
                return;
            }

            ModelImporter modelImporter = assetImporter as ModelImporter;
            //modelImporter.defaultClipAnimations = importRules.ImportSettings.texture.streamingMipmaps;
            //modelImporter.defaultClipAnimations[0].takeName
        }
        #endregion
    }
}
