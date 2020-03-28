namespace Mayfair.Core.Editor.VisualAsset
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.VisualAsset;
    using Mayfair.Core.Editor.GUI;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(VisualAssetData))]
    public class VisualAssetDataEditor : Editor
    {
        #region Class Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (targets.Length == 1)
            {
                DrawTarget(serializedObject, target as VisualAssetData);
            }
            else
            {
                foreach (UnityEngine.Object target in targets)
                {
                    DrawTarget(new SerializedObject(target), target as VisualAssetData);
                }
            }
        }

        public void DrawTarget(SerializedObject serializedObject, VisualAssetData target)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Data content validation");

            serializedObject.Update();

            VisualAssetMissingContent missingContent = VisualAssetMissingContent.Nothing;
            GameObject root = target.gameObject;
            VisualAssetData rootVisualData = target;
            {
                missingContent = ValidateSetup(rootVisualData);
            }

            bool shouldRebuildContent = false;
            Array values = Enum.GetValues(typeof(VisualAssetMissingContent));
            using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
            {
                foreach (VisualAssetMissingContent value in values)
                {
                    if (value == VisualAssetMissingContent.Nothing)
                    {
                        continue;
                    }

                    shouldRebuildContent = DrawContentMissing(missingContent, value) || shouldRebuildContent;
                }
            }

            if (GUILayout.Button(shouldRebuildContent ? "Repair asset data" : "Force rebuild asset data"))
            {
                root = target.gameObject;
                rootVisualData = target;
                {
                    GameObject basePlate = VisualAssetBuilderHelper.GetBasePlate(root);
                    VisualAssetLodReference foundRef = null;
                    VisualAssetLodReference[] lodRefs = root.GetComponentsInChildren<VisualAssetLodReference>();
                    foreach (VisualAssetLodReference lodRef in lodRefs)
                    {
                        if (lodRef.name == $"{VisualAssetBuilderHelper.LOD_NAME}0")
                        {
                            foundRef = lodRef;
                            break;
                        }
                    }

                    rootVisualData.Init(basePlate, foundRef.gameObject);

                    VisualAssetBuilderHelper.SetupBoxColliders(rootVisualData, basePlate);
                }

                EditorUtility.SetDirty(root);
                EditorUtility.SetDirty(rootVisualData);
            }

            serializedObject.ApplyModifiedProperties();
        }

        public bool DrawContentMissing(VisualAssetMissingContent contentMissing, VisualAssetMissingContent testContent)
        {
            bool result = false;
            int r = 0;
            Rect rect = EditorGUILayout.GetControlRect();
            Rect[] rects = RectHelper.SplitX(ref rect, Split.FixedSize(120), Split.FixedSize(10), 100);

            GUIHelper.ShowStatusBox(rects[r++], !contentMissing.HasEither(testContent));
            r++;
            EditorGUI.LabelField(rects[r++], ObjectNames.NicifyVariableName(testContent.ToString()));

            return result;
        }

        public static VisualAssetMissingContent ValidateSetup(VisualAssetData target)
        {
            VisualAssetMissingContent result = VisualAssetMissingContent.Nothing;

            ValidateSetup<GameObject>(target, ref result, VisualAssetMissingContent.basePlate);

            ValidateSetup<List<VisualAssetLodReference>>(target, ref result, VisualAssetMissingContent.lodReferences);

            ValidateSetup<List<Transform>>(target, ref result, VisualAssetMissingContent.animationTransforms);

            ValidateSetup<Animator>(target, ref result, VisualAssetMissingContent.Animator);

            ValidateSetup<VisualPlayableGraph>(target, ref result, VisualAssetMissingContent.VisualGraph);

            ValidateSetup<Collider>(target, ref result, VisualAssetMissingContent.Colliders);

            return result;
        }

        public static void ValidateSetup<T>(VisualAssetData target, ref VisualAssetMissingContent result, VisualAssetMissingContent content)
            where T : class
        {
            Type type = typeof(T);
            if (type == typeof(Collider) || type.IsSubclassOf(typeof(Collider)))
            {
                T[] instances = target.gameObject.GetComponentsInChildren<T>();
				//We need at least 2 colliders in the building
				// - 1 for the BasePlate
				// - 1+ for the building
                if (instances.Length < 2)
                {
                    result |= content;
                }
            }
            else if (type.IsSubclassOf(typeof(Component)))
            {
                T instance = target.gameObject.GetComponent<T>();
                if (instance == null)
                {
                    result |= content;
                }
            }
            else
            {
                ReflectedField<T> value = content.ToString();
                value.Init(target);

                if (value.Value == null || value.Value.Equals(null))
                {
                    result |= content;
                }

                if (typeof(IList).IsAssignableFrom(type))
                {
                    if ((value.Value as IList).Count == 0)
                    {
                        result |= content;
                    }
                }
            }
        }
        #endregion
    }
}
