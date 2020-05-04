namespace Mayfair.Core.Editor.Animation
{
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.Animation;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.Utils.Extensions;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Mayfair.Core.Editor.Addressables;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.Utils;
    using Mayfair.Core.Editor.VisualAsset;
    using UnityEditor;
    using UnityEngine;

    public static class AnimationBuilderHelper
    {
        #region Class Methods
        public static string[] GetAnimationsForAsset(string assetName)
        {
            assetName = Path.GetFileNameWithoutExtension(assetName);
            string newName = $"t:{typeof(AnimationClip).Name} {assetName}";
            string[] assets = AssetDatabase.FindAssets(newName);
            for (int a = 0; a < assets.Length; a++)
            {
                assets[a] = AssetDatabase.GUIDToAssetPath(assets[a]);
            }

            return assets;
        }

        public static string GetAnimationName(string assetName)
        {
            string takeName = string.Empty;
            if (Path.GetExtension(assetName) == $".{ConstsEditor.ANIM}")
            {
                return Path.GetFileNameWithoutExtension(assetName);
            }
            else if (assetName.Contains(ConstsEditor.ANIM_SEPARATOR))
            {
                assetName = Path.GetFileNameWithoutExtension(assetName);
                int index = assetName.LastIndexOf(ConstsEditor.ANIM_SEPARATOR[0]);
                takeName = assetName.Substring(index + 1);
                assetName = assetName.Substring(0, index);
                return $"Anim_{takeName}_{assetName}";
            }

            return string.Empty;
        }

        public static string GetAnimationFolder(string originalPath)
        {
            originalPath = PathHelper.Simplify(originalPath);
            string replaced = originalPath;
            string replacement = ConstsEditor.ANIMATIONS;
            int index = originalPath.LastIndexOf(Path.AltDirectorySeparatorChar);
            if (index > Consts.INDEX_NONE)
            {
                replaced = $"{Path.AltDirectorySeparatorChar}{originalPath.Substring(index + 1)}";
                replacement = $"{Path.AltDirectorySeparatorChar}{ConstsEditor.ANIMATIONS}";
            }

            return originalPath.Replace(replaced, replacement);
        }

        public static void GenerateCleanedUpAnimation(string clipPath, string destinationPath)
        {
            AnimationClip loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            AnimationClip newClip = Object.Instantiate(loadedClip);
            AnimationHelperContext context = new AnimationHelperContext(newClip);
            context.Init();

            for (int b = 0; b < context.clipBindings.Count; b++)
            {
                AnimationBindingsSetup binding = context.clipBindings[b];
                string name = binding.splitPath.Last();

                AssetLibraryItem item = AssetLibrary.GetItem(binding.splitPath.Last());
                if (item != null || MeshRegex.GetLODIndex(name) > 0 || RegexHelper.UnityImportRegex.Match(name).Success)
                {
                    context.clipBindings.RemoveAt(b--);
                }
            }

            List<string> keys = new List<string>(context.bindingPaths.Keys);
            keys.Sort();
            foreach (string key in keys)
            {
                string value = context.bindingPaths[key];
                int lodIndex = MeshRegex.GetLODIndex(value);
                if (lodIndex == Consts.INDEX_NONE)
                {
                    continue;
                }

                context.bindingPaths[key] = $"{VisualAssetBuilderHelper.ANIM_ROOT}/{VisualAssetBuilderHelper.ANIM_ROOT_CENTERED}/{VisualAssetBuilderHelper.LOD_GROUP}/{VisualAssetBuilderHelper.LOD_NAME}{lodIndex}";
            }

            context.ApplyPathChanges();

            newClip.SetCurve($"{VisualAssetBuilderHelper.ANIM_ROOT}/{VisualAssetBuilderHelper.ANIM_ROOT_CENTERED}", typeof(Transform), "m_LocalScale.x", AnimationCurve.Constant(0, 0, 1));
            newClip.SetCurve($"{VisualAssetBuilderHelper.ANIM_ROOT}/{VisualAssetBuilderHelper.ANIM_ROOT_CENTERED}", typeof(Transform), "m_LocalScale.y", AnimationCurve.Constant(0, 0, 1));
            newClip.SetCurve($"{VisualAssetBuilderHelper.ANIM_ROOT}/{VisualAssetBuilderHelper.ANIM_ROOT_CENTERED}", typeof(Transform), "m_LocalScale.z", AnimationCurve.Constant(0, 0, 1));

            AssetDatabase.CreateAsset(newClip, destinationPath);
            AssetDatabase.SaveAssets();
        }

        public static void GenerateAnimationLibrary(string placeSearchPath, List<string> createdClips, UniqueId nonAdditiveId)
        {
            string libraryName = Path.GetFileName(placeSearchPath);
            AnimationLibrary library = AssetMenuHelper.CreateAsset<AnimationLibrary>(Path.GetDirectoryName(placeSearchPath), libraryName, false);
            ReflectedField<List<AnimationClip>> standardClips = "standardClips";
            ReflectedField<List<AnimationClip>> additiveClips = "additiveClips";
            standardClips.Init(library);
            additiveClips.Init(library);

            foreach (string clip in createdClips)
            {
                AnimationClip loadedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clip);
                UniqueId clipId = new UniqueId(loadedClip.name);
                if (nonAdditiveId.Match(clipId) <= TagMatchResultType.MatchPartial)
                {
                    standardClips.Value.Add(loadedClip);
                }
                else
                {
                    additiveClips.Value.Add(loadedClip);
                }
            }

            AddressableHelper.SetAddressWithKeyword(library, AnimationLibraryResourceServiceProvider.KEYWORDS[Consts.FIRST_ITEM], libraryName);

            EditorUtility.SetDirty(library);
            AssetDatabase.SaveAssets();
        }
        #endregion
    }
}
