namespace Mayfair.Core.Editor.VisualAsset
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.Utils.Extensions;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Mayfair.Core.Code.VisualAsset;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.ObjectCategorizing;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEditor.Animations;
    using UnityEngine;
    using UnityEngine.Assertions;

    public static class VisualAssetBuilderHelper
    {
        #region Static and Constants
        private const string PROPS = "PropsGroup";
        public const string PROPS_KEY = "_Props_";
        public const string PROP_VISUAL_INSTANCE = "VisualPropInstance";
        private const string PROP_VISUAL_PROPID = "savedUniqueId";
        private const string PROP_CHILD_NAME = "Instance_";

        private const string ANIM_CONTROLLER = "AnimCtrl_BuildingStandard";
        public const string ANIM_ROOT = "AnimationRoot";
        public const string ANIM_ROOT_CENTERED = "CenteredRoot";

        public const string LOD_GROUP = "LodGroup";
        public const string LOD_NAME = "Lod";

        public const string LOD_RENDERERS = "lodRenderers";
        public const string LOD_FILTERS = "lodFilters";

        private const string ASSET_FILTER = "t:material";

        public static readonly CategoryContentType[] PotentialLods =
        {
            CategoryContentType.LOD0,
            CategoryContentType.LOD1,
            CategoryContentType.LOD2,
            CategoryContentType.LOD3
        };

        public static readonly Type[] CLEARABLE_COMPONENTS = {typeof(MeshRenderer), typeof(MeshFilter)};

        private static Type PropsBuildingInstanceType;
        private static Type visualPropInstanceType;
        #endregion

        #region Properties
        public static Type VisualPropInstanceType
        {
            get
            {
                if (visualPropInstanceType == null)
                {
                    visualPropInstanceType = AssemblyHelper.GetTypeFromAnyAssembly(PROP_VISUAL_INSTANCE);
                }

                return visualPropInstanceType;
            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        ///     Given a GameObject, this method does the following:
        ///     - Create a GameObject for the result prefab
        ///     - Find the Lod0, and remove all Mesh related component from the hierarchy
        ///     - Find the LodN names, load them from the art export folder, create a LodGroup and store the instances of the
        ///     loaded prefab in there
        ///     - Load the prefab named $"{visualSource.name}_{ConstsEditor.UNIQUE} and instantiate/fit its children in the new
        ///     root
        ///     - Retrieve all the Props identified by the AssetLibrary, and nest the replacement prefabs in the new root
        /// </summary>
        /// <param name="visualSource">The original GameObject</param>
        /// <param name="sourcePath">The original asset path of the given GameObject</param>
        /// <returns></returns>
        public static GameObject GenerateVisualAssetPrefab(GameObject visualSource, string sourcePath, GUILogger logger = null)
        {
            string error = string.Empty;
            string task = string.Empty;
            if (logger != null)
            {
                error = logger.GetTintPrefix(0).value;
                task = logger.GetTintPrefix(4).value;
            }

            GameObject prefabResult = new GameObject(visualSource.name);
            VisualAssetData visualAssetData = prefabResult.AddComponent<VisualAssetData>();

            List<CategorizedInstance> categorizedSources = new List<CategorizedInstance>();
            GameObject uniqueContent = null;
            List<GameObject> lodContent = new List<GameObject>();

            //Try to load the merged-exported meshes
            bool fillOperationSucceeded = TryLoadingDependencies(visualSource, sourcePath, categorizedSources, out uniqueContent, lodContent, logger, error);

            GameObject emptyLod0 = null; //AddEmptyLod0(prefabResult, categorizedSources);
            GameObject basePlate = null;

            if (fillOperationSucceeded)
            {
                AddLoadedUniqueContent(prefabResult, uniqueContent);
                basePlate = GetBasePlate(prefabResult);
            }

            if (logger != null)
            {
                if (fillOperationSucceeded)
                {
                    logger.Log($"{task} Successfully loaded dependencies, found:");
                }
                else
                {
                    logger.Log($"{error} Failed loading dependencies, found only:");
                }

                foreach (GameObject gameObject in lodContent)
                {
                    if (gameObject != null)
                    {
                        logger.Log($"- {gameObject.name}");
                    }
                }

                logger.Log(uniqueContent == null ? "- Unique content missing" : $"- {uniqueContent.name}");
            }

            //Setup animation root
            GameObject rootRotation = null;
            PrepareAnimationRoots(prefabResult, ref rootRotation);

            //Setup LOD groups
            GameObject lodParent = null;
            PrepareLodGroup(rootRotation, ref lodParent, lodContent.Count);

            Material[] materials = null;
            LoadMaterials(prefabResult, ref materials);

            //Directly exit, no need to post process now, we'll come back here after the lod have been generated
            if (fillOperationSucceeded)
            {
                //Add all pre-generated objects to LOD group
                FillLodGroupWithLoadedLods(prefabResult, lodParent, materials, lodContent);

                if (basePlate != null && materials != null)
                {
                    MeshRenderer[] renderers = basePlate.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer renderer in renderers)
                    {
                        renderer.sharedMaterials = materials;
                    }
                }
            }

            //Replace all the prop hooks with their real counterpart
            MatchAssetLibraryItems(prefabResult, logger);

            if (fillOperationSucceeded)
            {
                visualAssetData.Init(basePlate, lodContent[0]);
            }

            SetupBoxColliders(visualAssetData, basePlate);

            return prefabResult;
        }

        private static GameObject AddEmptyLod0(GameObject parent, List<CategorizedInstance> originalMatches)
        {
            //Clean up the lod0 of any mesh related things
            List<CategorizedInstance> keptRoots = new List<CategorizedInstance>();
            CategorizerManipulator.GatherLodBranches(originalMatches, keptRoots, CategoryContentType.LOD0);
            if (keptRoots.Count > 0)
            {
                GameObject original = keptRoots[Consts.FIRST_ITEM].originalTransform.gameObject;
                GameObject keptRoot = UnityEngine.Object.Instantiate(original, parent.transform);
                keptRoot.name = original.name;

                //A bit brutal, but very efficient
                foreach (Type type in CLEARABLE_COMPONENTS)
                {
                    keptRoot.ClearComponents(type, true);
                }

                keptRoot.transform.Reset();

                return keptRoot;
            }

            return null;
        }

        private static void AddLoadedUniqueContent(GameObject parent, GameObject uniqueContent)
        {
            for (int c = 0; c < uniqueContent.transform.childCount; c++)
            {
                Transform child = uniqueContent.transform.GetChild(c);
                Transform newChild = UnityEngine.Object.Instantiate(child, parent.transform);
                newChild.name = child.name;
                newChild.transform.CopyLocalValues(child);
            }
        }

        public static GameObject GetBasePlate(GameObject parent)
        {
            for (int c = 0; c < parent.transform.childCount; c++)
            {
                Transform child = parent.transform.GetChild(c);
                if (child.name == ConstsEditor.BASE_PLATE)
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        private static void PrepareAnimationRoots(GameObject parent, ref GameObject rootRotation, GUILogger logger = null, string error = "")
        {
            string animatorName = typeof(AnimatorController).Name;
            string[] foundAssets = AssetDatabase.FindAssets($"t:{animatorName} {ANIM_CONTROLLER}");
            AnimatorController controller = null;
            foreach (string foundAsset in foundAssets)
            {
                AnimatorController newController = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(foundAsset));
                if (newController == null)
                {
                    continue;
                }

                if (controller != null && logger != null)
                {
                    logger.Log($"{error} Error: Found 2 {animatorName} with the same name {controller.name}");
                    break;
                }

                controller = newController;
            }

            GameObject rootPosition = new GameObject(ANIM_ROOT);
            rootPosition.transform.SetParent(parent.transform, false);
            rootPosition.transform.Reset();

            rootRotation = new GameObject(ANIM_ROOT_CENTERED);
            rootRotation.transform.SetParent(rootPosition.transform, false);
            rootRotation.transform.Reset();
        }

        private static void PrepareLodGroup(GameObject parent, ref GameObject lodParent, int lodCount)
        {
            float[] lodValues = AssetLibrary.GetLodValues();

            lodParent = new GameObject(LOD_GROUP);
            lodParent.transform.SetParent(parent.transform, false);
            lodParent.transform.Reset();

            LODGroup lodGroup = lodParent.AddComponent<LODGroup>();
            LOD[] lods = new LOD[lodCount];
            for (int l = 0; l < lods.Length; l++)
            {
                float lodValue = 0f;
                if (lodValues == null)
                {
                    lodValue = 1f / (l + 1f);
                }
                else
                {
                    lodValue = lodValues[Mathf.Min(l, lodValues.Length - 1)];
                }

                LOD lod = lods[l];
                lod = new LOD(lodValue, null);
                lods[l] = lod;
            }

            lodGroup.SetLODs(lods);
        }

        private static void LoadMaterials(GameObject parent, ref Material[] materials)
        {
            string[] foundAssets = AssetDatabase.FindAssets($"{parent.name} {ASSET_FILTER}");
            if (foundAssets != null && foundAssets.Length > 0)
            {
                List<Material> tempMaterials = new List<Material>();
                for (int m = 0; m < foundAssets.Length; m++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(foundAssets[m]);
                    Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
                    if (material.name == parent.name)
                    {
                        tempMaterials.Add(material);
                    }
                }

                materials = tempMaterials.ToArray();
            }
        }

        private static void FillLodGroupWithLoadedLods(GameObject parent, GameObject lodParent, Material[] materials, List<GameObject> generatedObjs)
        {
            VisualAssetData visualData = parent.GetComponent<VisualAssetData>();
            LODGroup lodGroup = lodParent.GetComponent<LODGroup>();
            LOD[] lods = lodGroup.GetLODs();

            for (int g = 0; g < generatedObjs.Count; g++)
            {
                GameObject genObj = generatedObjs[g];
                GameObject newObj = UnityEngine.Object.Instantiate(genObj);
                newObj.transform.SetParent(lodParent.transform, false);
                newObj.transform.Reset();
                generatedObjs[g] = newObj;

                int lodIndex = MeshRegex.GetLODIndex(genObj.name);
                LOD lod = lods[lodIndex];
                newObj.name = $"{LOD_NAME}{lodIndex}";

                VisualAssetLodReference lodReference = newObj.AddComponent<VisualAssetLodReference>();
                lodReference.Init(visualData.AnimationTransforms);

                if (materials != null)
                {
                    foreach (MeshRenderer meshRenderer in lodReference.LodRenderers)
                    {
                        meshRenderer.sharedMaterials = materials;
                    }
                }

                lod.renderers = lodReference.LodRenderers;

                lods[lodIndex] = lod;
            }

            lodGroup.SetLODs(lods);

            //We know the game is aligned on the grid, and that building size is expressed in GridCell if size == 1 unity-unit
            Bounds bounds = GetBounds(parent, true);
            Vector3 extents = bounds.extents;
            extents.y = 0;
            extents *= 2;
            extents = new Vector3(Mathf.RoundToInt(extents.x) - 1, 0, Mathf.RoundToInt(extents.z) - 1) * 0.5f;
            lodGroup.transform.parent.localPosition = extents;
            lodGroup.transform.localPosition = -extents;
        }

        public static void SetupBoxColliders(VisualAssetData visualAssetData, GameObject basePlate)
        {
            List<MeshRenderer> renderers = new List<MeshRenderer>();

            if (basePlate != null)
            {
                AddBoxCollider(basePlate, basePlate.GetComponent<MeshRenderer>());
            }

            GameObject colliderRoot = null;
            renderers.Clear();
            foreach (VisualAssetLodReference lodRefererence in visualAssetData.LodReferences)
            {
                colliderRoot = lodRefererence.transform.parent.gameObject;
                renderers.AddRange(lodRefererence.LodRenderers);
            }

            if (renderers.Count > 0)
            {
                AddBoxCollider(colliderRoot, renderers);
            }
        }

        private static Bounds GetBounds(GameObject root, bool includeChildren = false)
        {
            MeshFilter[] filters = includeChildren ? null : new MeshFilter[1];
            if (!includeChildren)
            {
                filters[0] = root.GetComponent<MeshFilter>();
            }
            else
            {
                filters = root.GetComponentsInChildren<MeshFilter>();
            }

            return GetBounds(root, filters);
        }

        private static Bounds GetBounds(GameObject root, MeshFilter[] filters)
        {
            bool hasInit = false;
            Bounds bounds = new Bounds();
            Transform rootTransform = root.transform;
            foreach (MeshFilter filter in filters)
            {
                filter.sharedMesh.RecalculateBounds();
                Vector3 min = filter.sharedMesh.bounds.min;
                Vector3 center = filter.sharedMesh.bounds.center;
                Vector3 max = filter.sharedMesh.bounds.max;
                min = rootTransform.InverseTransformPoint(filter.transform.TransformPoint(min));
                center = rootTransform.InverseTransformPoint(filter.transform.TransformPoint(center));
                max = rootTransform.InverseTransformPoint(filter.transform.TransformPoint(max));

                Bounds otherBounds = new Bounds(center, Vector3.zero);
                otherBounds.Encapsulate(min);
                otherBounds.Encapsulate(max);

                if (!hasInit)
                {
                    bounds = otherBounds;
                    hasInit = true;
                }
                else
                {
                    bounds.Encapsulate(otherBounds);
                }
            }

            return bounds;
        }

        private static void AddBoxCollider(GameObject root, MeshRenderer source)
        {
            if (source == null)
            {
                return;
            }

            MeshFilter[] filters = {root.GetComponent<MeshFilter>()};
            Bounds bounds = GetBounds(root, filters);

            root.ClearComponents<Collider>();
            BoxCollider collider = root.AddComponent<BoxCollider>();
            collider.center = bounds.center;
            collider.size = bounds.size;
            collider.gameObject.layer = LayerUtils.LAYER_BUILDING_GHOST;
            EditorUtility.SetDirty(collider.gameObject);
        }

        private static void AddBoxCollider(GameObject root, List<MeshRenderer> sources)
        {
            List<Bounds> validBounds = new List<Bounds>();
            foreach (MeshRenderer source in sources)
            {
                MeshFilter[] filters = {source.GetComponent<MeshFilter>()};
                validBounds.Add(GetBounds(root, filters));
            }

            BoundsHelpers.TrySimplifyBounds(validBounds);

            root.ClearComponents<Collider>();
            foreach (Bounds validBound in validBounds)
            {
                BoxCollider collider = root.AddComponent<BoxCollider>();
                collider.center = validBound.center;
                collider.size = validBound.size;
                collider.gameObject.layer = LayerUtils.LAYER_BUILDING_GHOST;
                EditorUtility.SetDirty(collider.gameObject);
            }
        }

        private static void AddBoxColliderFromVoxels(GameObject root, List<MeshRenderer> sources)
        {
            Vector3 boundsGranularity = Vector3.one * 0.1f;

            Transform rootTransform = root.transform;
            Bounds rootBounds = GetBounds(root, true);
            Bounds granularBounds = rootBounds;
            Vector3 rootMin = granularBounds.min;
            Vector3 rootMax = granularBounds.max;
            rootMin = new Vector3(Mathf.FloorToInt(rootMin.x), Mathf.FloorToInt(rootMin.y), Mathf.FloorToInt(rootMin.z));
            rootMax = new Vector3(Mathf.CeilToInt(rootMax.x), Mathf.CeilToInt(rootMax.y), Mathf.CeilToInt(rootMax.z));
            granularBounds = new Bounds(rootMin, Vector3.zero);
            granularBounds.Encapsulate(rootMax);
            float xMax = granularBounds.size.x / boundsGranularity.x;
            float yMax = granularBounds.size.y / boundsGranularity.y;
            float zMax = granularBounds.size.z / boundsGranularity.z;

            List<Bounds> validBounds = new List<Bounds>();
            List<int> validIndices = new List<int>();
            for (int yI = 0; yI < yMax; yI++)
            {
                for (int zI = 0; zI < zMax; zI++)
                {
                    for (int xI = 0; xI < xMax; xI++)
                    {
                        float x = granularBounds.min.x + xI * boundsGranularity.x;
                        float y = granularBounds.min.y + yI * boundsGranularity.y;
                        float z = granularBounds.min.z + zI * boundsGranularity.z;
                        Bounds cellBounds = new Bounds(new Vector3(x, y, z) + boundsGranularity * 0.5f, Vector3.zero);
                        cellBounds.extents = boundsGranularity * 0.5f;
                        validBounds.Add(cellBounds);
                        validIndices.Add(0);
                    }
                }
            }

            float distance = boundsGranularity.x / 2f;
            foreach (MeshRenderer renderer in sources)
            {
                MeshFilter filter = renderer.GetComponent<MeshFilter>();
                Mesh mesh = filter.sharedMesh;
                int[] indices = mesh.GetIndices(0);
                Vector3[] vertices = mesh.vertices;

                for (int i = 0; i < indices.Length; i += 3)
                {
                    Vector3 v0 = vertices[indices[i]];
                    Vector3 v1 = vertices[indices[i + 1]];
                    Vector3 v2 = vertices[indices[i + 2]];
                    float d01 = (v1 - v0).magnitude;
                    float d21 = (v1 - v2).magnitude;
                    do
                    {
                        Vector3 v01 = v0 + (v1 - v0).normalized * d01;
                        Vector3 v21 = v2 + (v1 - v2).normalized * d21;
                        float d2101 = (v01 - v21).magnitude;
                        do
                        {
                            Vector3 v2101 = v21 + (v01 - v21).normalized * d2101;

                            Vector3 vertex = v2101;

                            //foreach (Vector3 vertex in vertices)
                            {
                                Vector3 localVertex = vertex;
                                localVertex = rootTransform.InverseTransformPoint(filter.transform.TransformPoint(localVertex));
                                Vector3 arrayVertex = localVertex - granularBounds.min;
                                arrayVertex.x /= boundsGranularity.x;
                                arrayVertex.y /= boundsGranularity.y;
                                arrayVertex.z /= boundsGranularity.z;

                                int vi = (int) ((int) arrayVertex.x + (int) arrayVertex.z * xMax + (int) arrayVertex.y * xMax * zMax);
                                if (validBounds[vi].Contains(localVertex))
                                {
                                    validIndices[vi] = 1;
                                }
                            }

                            if (Mathf.Approximately(0, d2101))
                            {
                                break;
                            }

                            d2101 = Mathf.Max(0, d2101 - distance);
                        } while (d2101 >= 0);

                        d01 = Mathf.Max(0, d01 - distance);
                        d21 = Mathf.Max(0, d21 - distance);
                    } while (d01 > 0 || d21 > 0);
                }
            }

            for (int i = validIndices.Count - 1; i >= 0; i--)
            {
                if (validIndices[i] == 0)
                {
                    validBounds.RemoveAt(i);
                }
            }

            BoundsHelpers.TryMergingBounds(validBounds);

            foreach (Bounds validBound in validBounds)
            {
                BoxCollider collider = root.AddComponent<BoxCollider>();
                collider.center = validBound.center;
                collider.size = validBound.size;
                collider.gameObject.layer = LayerUtils.LAYER_BUILDING_GHOST;
                EditorUtility.SetDirty(collider.gameObject);
            }
        }

        private static void PreparePropsParent(GameObject parent, out GameObject propsParent)
        {
            propsParent = new GameObject($"{PROPS}");
            propsParent.transform.SetParent(parent.transform, false);
            propsParent.transform.Reset();
        }

        private static void MatchAssetLibraryItems(GameObject prefabResult, GUILogger logger = null)
        {
            List<CategorizedInstance> instances = new List<CategorizedInstance>();
            GameObjectCategorizer.Gather(prefabResult, instances);
            GameObjectCategorizer.Identify(instances);

            string error = string.Empty;
            string task = string.Empty;
            if (logger != null)
            {
                error = logger.GetTintPrefix(0).value;
                task = logger.GetTintPrefix(4).value;
            }

            if (logger != null)
            {
                logger.Log($"{task} Replacing props");
            }

            for (int m = 0; m < instances.Count; m++)
            {
                CategorizedInstance categorized = instances[m];
                if (categorized.originalTransform == null)
                {
                    continue;
                }

                GameObject newChild = AssetLibrary.TryReplacing(categorized.originalTransform.gameObject);
                if (newChild != null)
                {
                    if (logger != null)
                    {
                        logger.Log($"Found Prop: {newChild.name}, was {categorized.Name}");
                    }

                    //Add an instance parent to the prop
                    GameObject propGameObject = new GameObject($"{PROP_CHILD_NAME}{newChild.name}");
                    propGameObject.transform.SetParent(newChild.transform.parent);
                    propGameObject.transform.CopyLocalValues(newChild.transform);
                    newChild.transform.SetParent(propGameObject.transform);
                    newChild.transform.Reset();

                    //Add the actual prop instance
                    Component propInstance = propGameObject.AddComponent(VisualPropInstanceType);
                    ReflectedField<SerializableUniqueId> propUniqueId = PROP_VISUAL_PROPID;
                    propUniqueId.Init(propInstance);
                    propUniqueId.Value = newChild.name;

                    for (int c = 0; c < instances.Count; c++)
                    {
                        CategorizedInstance child = instances[c];
                        if (child.originalTransform != null && child.originalTransform.HasGivenParent(categorized.originalTransform))
                        {
                            child.originalTransform = null;
                        }
                    }
                }
                else
                {
                    if (logger != null && (categorized.category.HasBoth(CategoryContentType.PROP) || categorized.Name.Contains(PROPS_KEY)))
                    {
                        logger.Log($"{error} Identified Prop: {categorized.Name} but couldn't find a replacement");
                    }
                }
            }
        }

        private static bool TryLoadingDependencies(GameObject visualSource, string sourcePath, List<CategorizedInstance> originalMatches, out GameObject cleanObject, List<GameObject> generatedObjs, GUILogger logger, string error)
        {
            GameObjectCategorizer.Gather(visualSource.gameObject, originalMatches);
            GameObjectCategorizer.Identify(originalMatches);

            //Try to load the merged-exported meshes
            return TryLoadingDependencies(sourcePath, originalMatches, out cleanObject, generatedObjs, logger, error);
        }

        private static bool TryLoadingDependencies(string sourcePath, List<CategorizedInstance> originalMatches, out GameObject cleanObject, List<GameObject> loadedObjects, GUILogger logger, string error)
        {
            cleanObject = null;

            //Find lod roots
            CategorizedInstance firstRoot = null;
            List<CategorizedInstance> roots = new List<CategorizedInstance>();
            CategorizerManipulator.GatherLodRoot(originalMatches, roots);

            bool success = true;
            for (int r = 0; r < PotentialLods.Length + 1; r++)
            {
                bool presenceMandatory = true;
                string searchName = string.Empty;
                if (r == 0)
                {
                    searchName = $"{Path.GetFileNameWithoutExtension(sourcePath)}_{ConstsEditor.UNIQUE}";
                }
                else
                {
                    int realR = r - 1;
                    if (realR < roots.Count)
                    {
                        CategorizedInstance root = roots[r - 1];
                        if (firstRoot == null)
                        {
                            firstRoot = root;
                        }

                        searchName = root.Name;
                    }
                    else
                    {
                        presenceMandatory = false;
                        CategoryContentType searchLod = PotentialLods[realR];
                        int searchLodIndex = MeshRegex.GetLODIndex($"A_{searchLod.ToString()}");
                        int rootLodIndex = MeshRegex.GetLODIndex(firstRoot.Name);
                        searchName = firstRoot.Name.Replace($"{ConstsEditor.LOD}{rootLodIndex}", $"{ConstsEditor.LOD}{searchLodIndex}");
                    }
                }

                string searchPath = ExporterHelper.GetExportAssetPath(sourcePath, searchName);
                GameObject foundAsset = AssetDatabase.LoadAssetAtPath<GameObject>(searchPath);
                if (foundAsset == null || foundAsset.GetComponentInChildren<MeshFilter>() == null)
                {
                    if (!presenceMandatory)
                    {
                        continue;
                    }

                    if (foundAsset == null)
                    {
                        logger.Log($"{error} Failed loading dependencies: {searchPath}");
                    }
                    else
                    {
                        logger.Log($"{error} Loaded dependency but found no {typeof(MeshFilter).Name}: {searchPath}");
                    }

                    success = false;
                    continue;
                }

                if (r == 0)
                {
                    cleanObject = foundAsset;
                }
                else
                {
                    loadedObjects.Add(foundAsset);
                }
            }

            return success;
        }

        public static void Init(this VisualAssetData assetData, GameObject basePlate, GameObject animationRoot)
        {
            GameObject gameObject = assetData.gameObject;
            ReflectedField<GameObject> basePlateRef = VisualAssetMissingContent.basePlate.ToString();
            basePlateRef.Init(assetData);

            ReflectedField<List<Transform>> animationTransforms = VisualAssetMissingContent.animationTransforms.ToString();
            animationTransforms.Init(assetData);

            basePlateRef.Value = basePlate;

            ReflectedField<List<VisualAssetLodReference>> lodReferences = VisualAssetMissingContent.lodReferences.ToString();
            lodReferences.Init(assetData);

            lodReferences.Value.Clear();
            lodReferences.Value.AddRange(gameObject.GetComponentsInChildren<VisualAssetLodReference>());
            lodReferences.Value.Sort((a, b) =>
            {
                return string.Compare(a.name, b.name);
            });

            if (animationRoot != null)
            {
                animationTransforms.Value.Clear();
                animationRoot.transform.GatherChildrenOrdered(animationTransforms);
                RemoveProps(animationTransforms.Value);
            }

            for (int r = 0; r < lodReferences.Value.Count; r++)
            {
                VisualAssetLodReference lodReference = lodReferences.Value[r];
                lodReference.Init(animationTransforms);
            }

            Animator animator = assetData.gameObject.GetOrAddComponent<Animator>();
            animator.runtimeAnimatorController = null;

            assetData.gameObject.GetOrAddComponent<VisualPlayableGraph>();
        }

        public static void Init(this VisualAssetLodReference lodReference, List<Transform> referenceTransforms)
        {
            GameObject gameObject = lodReference.gameObject;
            List<Transform> foundTransforms = new List<Transform>(gameObject.GetComponentsInChildren<Transform>());
            RemoveProps(foundTransforms);

            ReflectedField<List<Transform>> animationTransforms = VisualAssetMissingContent.animationTransforms.ToString();
            animationTransforms.Init(lodReference);
            animationTransforms.Value = new List<Transform>(referenceTransforms);

            //We allow null spaces in the animation transforms, as lower lods don't necessarly have the same amount of bones
            //The reference transforms are ordered by "layer of parent" so if children are missing, they should be at the end of the list
            //Thus reducing the risk for empty spaces in the middle.
            for (int t = 0; t < referenceTransforms.Count; t++)
            {
                string realName = referenceTransforms[t].name;
                bool refIsLod = MeshRegex.GetLODIndex(realName) > Consts.INDEX_NONE || realName.StartsWith(LOD_NAME);

                animationTransforms.Value[t] = foundTransforms.Find(x =>
                {
                    if (refIsLod)
                    {
                        bool xIsLod = x.name.StartsWith(LOD_NAME);
                        if (xIsLod)
                        {
                            return true;
                        }
                    }

                    return x.name == realName;
                });
            }

            RetrieveComponents<MeshRenderer>(lodReference, gameObject, LOD_RENDERERS);
            RetrieveComponents<MeshFilter>(lodReference, gameObject, LOD_FILTERS);
        }

        private static void RetrieveComponents<T>(object container, GameObject gameObject, string fieldName)
            where T : Component
        {
            ReflectedField<T[]> reflectedArray = fieldName;
            reflectedArray.Init(container);

            T[] foundComponents = gameObject.GetComponentsInChildren<T>();
            RemoveProps(foundComponents);
            List<T> componentList = new List<T>(foundComponents);
            componentList.RemoveAll(x => { return x == null; });

            reflectedArray.Value = componentList.ToArray();
        }
        
        public static void RemoveProps<T, U>(T[] array)
            where T : Component
            where U : Component
        {
            RemoveProps(array, typeof(U));
        }

        public static void RemoveProps<T>(T[] array)
            where T : Component
        {
            RemoveProps(array, VisualPropInstanceType);
        }

        private static void RemoveProps<T>(T[] array, Type type)
            where T : Component
        {
            Assert.IsTrue(type.IsSubclassOf(typeof(Component)));

            for (int a = 0; a < array.Length; a++)
            {
                Component foundComp = array[a].GetComponent(type);
                if (foundComp == null)
                {
                    foundComp = array[a].GetComponentInParent(type);
                }

                if (foundComp != null)
                {
                    array[a] = null;
                }
            }
        }

        public static void RemoveProps<T>(List<T> list)
            where T : Component
        {
            for (int a = 0; a < list.Count; a++)
            {
                Component foundComp = list[a].GetComponent(VisualPropInstanceType);
                if (foundComp == null)
                {
                    foundComp = list[a].GetComponentInParent(VisualPropInstanceType);
                }

                if (foundComp != null)
                {
                    list.RemoveAt(a--);
                }
            }
        }
        #endregion
    }
}
