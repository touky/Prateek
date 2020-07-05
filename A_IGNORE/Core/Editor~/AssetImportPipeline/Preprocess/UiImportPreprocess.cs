namespace Mayfair.Core.Editor.AssetImportPipeline.Preprocess
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class UiImportPreprocess : AssetPostprocessor
    {
        private const string UI_DIRECTORY = "Assets/Art/UI";

        private static readonly string[] TRANSPARENT_EXTENSIONS = new[]
        {
            ".png"
        };

        // PPU = Target Device Screen Height / (Camera Ortho Size * 2)
        // 144 = 1440 / (5 * 2)
        // Target device is iPhone x, Camera Ortho size is default (saw no reason to change it)
        // https://blogs.unity3d.com/2018/11/19/choosing-the-resolution-of-your-2d-art-assets/
        private const int PIXELS_PER_UNIT = 144;

        private void OnPreprocessTexture()
        {
            if (!IsAssetUi())
            {
                return;
            }

            if (IsNewAsset())
            {
                TextureImporter textureImporter = assetImporter as TextureImporter;
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spritePixelsPerUnit = PIXELS_PER_UNIT;
                SetTransparencySupport(ref textureImporter);
            }
        }

        private void SetTransparencySupport(ref TextureImporter textureImporter)
        {
            foreach (string extension in TRANSPARENT_EXTENSIONS)
            {
                if (Path.GetExtension(assetPath) == extension)
                {
                    textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
                    return;
                }
            }

            textureImporter.alphaSource = TextureImporterAlphaSource.None;
        }

        private bool IsAssetUi()
        {
            return assetPath.Contains(UI_DIRECTORY);
        }

        private bool IsNewAsset()
        {
            Object textureAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
            return textureAsset == null;
        }
    }
}