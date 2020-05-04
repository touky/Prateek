namespace Mayfair.Core.Editor.FBXExporter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Extensions;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Code.VisualAsset;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.ObjectCategorizing;
    using Mayfair.Core.Editor.Utils;
    using Mayfair.Core.Editor.VisualAsset;
    using UnityEditor;
    using UnityEngine;

    public static class FbxExportHelper
    {
        #region Static and Constants
        private static readonly CategoryContentType[] exportLODs =
        {
            CategoryContentType.LOD0,
            CategoryContentType.LOD1,
            CategoryContentType.LOD2,
            CategoryContentType.LOD3,
            CategoryContentType.LODN
        };

        private static CategoryContentType BranchCleanUp =
            CategoryContentType.LODALL |
            CategoryContentType.LOD1 |
            CategoryContentType.LOD2 |
            CategoryContentType.LOD3 |
            CategoryContentType.LODN;
        #endregion

        #region Class Methods
        public static void CreateUniqueContent(List<CategorizedInstance> CategorizedInstances, List<ExportContent> exportedContents)
        {
            List<CategorizedInstance> exportedInstances = new List<CategorizedInstance>(CategorizedInstances);
            GameObject source = UnityEngine.Object.Instantiate(CategorizedInstances[0].originalTransform).gameObject;

            GameObjectCategorizer.Gather(source, exportedInstances);
            GameObjectCategorizer.Identify(exportedInstances);

            List<CategorizedInstance> branches = new List<CategorizedInstance>();

            CategorizerManipulator.GatherBranches(exportedInstances, branches, BranchCleanUp, true);
            if (branches.Count == 0)
            {
                return;
            }

            foreach (CategorizedInstance branch in branches)
            {
                int index = exportedInstances.FindIndex(x =>
                {
                    return branch.originalTransform == x.originalTransform;
                });

                CategorizedInstance child = exportedInstances[index];
                if (child.originalTransform == null)
                {
                    continue;
                }

                child.originalTransform.SetParent(null);
                UnityEngine.Object.DestroyImmediate(child.originalTransform.gameObject);
            }

            exportedInstances[0].originalTransform.gameObject.ClearComponents<VisualAssetValidator>();

            RemoveEmptyNonDummyObjects(exportedInstances[0].originalTransform);

            List<GameObject> exportedObject = new List<GameObject>();
            foreach (CategorizedInstance instance in exportedInstances)
            {
                if (instance.originalTransform == null)
                {
                    continue;
                }

                if (instance.category.HasBoth(CategoryContentType.PROP))
                {
                    //A bit brutal, but very efficient
                    foreach (Type type in VisualAssetBuilderHelper.CLEARABLE_COMPONENTS)
                    {
                        instance.originalTransform.gameObject.ClearComponents(type, true);
                    }
                }

                exportedObject.Add(instance.originalTransform.gameObject);
            }

            string originalPath = AssetDatabase.GetAssetPath(CategorizedInstances[0].originalTransform);

            exportedContents.Add(new FBXExportContent
            {
                OriginalPath = originalPath,
                FileName = $"{Path.GetFileNameWithoutExtension(originalPath)}_{ConstsEditor.UNIQUE}",
                ExportedContent = exportedObject
            });
        }

        private static void RemoveEmptyNonDummyObjects(Transform root)
        {
            List<Transform> transforms = new List<Transform>();
            root.GatherChildrenOrdered(transforms, false);

            for (int t = 0; t < transforms.Count; t++)
            {
                Transform transform = transforms[t];
                if (transform == root)
                {
                    break;
                }

                bool isEmpty = transform.childCount == 0;
                if (isEmpty)
                {
                    isEmpty = !transform.name.EndsWith(ConstsEditor.DUMMY);
                }

                if (isEmpty)
                {
                    foreach (Type type in VisualAssetBuilderHelper.CLEARABLE_COMPONENTS)
                    {
                        Component component = transform.GetComponent(type);
                        if (component != null)
                        {
                            isEmpty = false;
                            break;
                        }
                    }
                }

                if (isEmpty)
                {
                    transform.SetParent(null);
                    UnityEngine.Object.DestroyImmediate(transform.gameObject);
                }
            }
        }

        public static void CreateLodContent(List<CategorizedInstance> CategorizedInstances, List<ExportContent> exportedContents)
        {
            List<CategorizedInstance> branches = new List<CategorizedInstance>();

            //Gather all the gameobjects necessary to export
            foreach (CategoryContentType matchContentType in exportLODs)
            {
                CategorizerManipulator.GatherLodBranches(CategorizedInstances, branches, matchContentType);
                if (branches.Count == 0)
                {
                    continue;
                }

                CategorizedInstance rootBranch = branches[Consts.FIRST_ITEM];
                if (matchContentType == CategoryContentType.LODN)
                {
                    if (rootBranch.category.HasBoth(CategoryContentType.LODN))
                    {
                        Debug.Assert(false, "Exporter only supports up to 4 lods right now, contact your local programmer to fix this");
                    }

                    return;
                }

                VisualAssetLodReference rootLOD = null;
                List<GameObject> foundObjects = new List<GameObject>();
                for (int b = 0; b < branches.Count; b++)
                {
                    CategorizedInstance branch = branches[b];
                    if (branch.originalTransform == null)
                    {
                        continue;
                    }

                    string branchName = branch.Name;
                    int lodIndex = MeshRegex.GetLODIndex(branch.Name);
                    if (lodIndex > Consts.INDEX_NONE)
                    {
                        branchName = $"{CategorizedInstances[0].Name}_{ConstsEditor.LOD}{MeshRegex.GetLODIndex(branch.Name)}";
                    }

                    branch.matchedTransform = new GameObject(branchName).transform;

                    if (branch.Parent != null)
                    {
                        CategorizedInstance parent = branch.Parent;

                        Debug.Assert(parent.matchedTransform != null);

                        branch.matchedTransform.SetParent(parent.matchedTransform);
                        branch.matchedTransform.localPosition = branch.originalTransform.localPosition;
                        branch.matchedTransform.localRotation = branch.originalTransform.localRotation;
                        branch.matchedTransform.localScale = branch.originalTransform.localScale;
                    }

                    if (!branch.category.HasBoth(CategoryContentType.PROP))
                    {
                        MeshFilter filter = branch.originalTransform.GetComponent<MeshFilter>();
                        if (filter != null)
                        {
                            MeshFilter newFilter = branch.matchedTransform.gameObject.AddComponent<MeshFilter>();
                            newFilter.sharedMesh = filter.sharedMesh;
                        }
                    }


                    foundObjects.Add(branch.matchedTransform.gameObject);
                }

                if (foundObjects.Count > 0)
                {
                    GameObject root = foundObjects[0];
                    MeshFilter filter = root.GetComponentInChildren<MeshFilter>();
                    if (filter == null)
                    {
                        Debug.Assert(filter != null);
                        return;
                    }
                    else
                    {
                        string assetPath = AssetDatabase.GetAssetPath(filter.sharedMesh == null ? filter.mesh : filter.sharedMesh);
                        exportedContents.Add(new FBXExportContent
                        {
                            OriginalPath = assetPath,
                            FileName = root.name,
                            ExportedContent = foundObjects
                        });
                    }
                }
            }
        }
        #endregion
    }
}
