namespace Mayfair.Core.Editor.ObjectCategorizing
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Editor.AssetLibrary;
    using UnityEngine;

    public static class GameObjectCategorizer
    {
        #region Class Methods
        public static void Gather(GameObject source, List<CategorizedInstance> matches)
        {
            Transform[] children = source.transform.GetComponentsInChildren<Transform>();

            matches.Clear();
            foreach (Transform child in children)
            {
                matches.Add(child);
            }

            foreach (CategorizedInstance match in matches)
            {
                match.Parent = matches.Find(x => x.originalTransform == match.originalTransform.parent);
            }
        }

        public static void Identify(List<CategorizedInstance> instances)
        {
            foreach (CategorizedInstance instance in instances)
            {
                instance.category = CategoryContentType.Nothing;

                int lodIndex = MeshRegex.GetLODIndex(instance.originalTransform.name);
                if (lodIndex > Consts.INDEX_NONE)
                {
                    switch (lodIndex)
                    {
                        case 0:
                        {
                            instance.category.Add(CategoryContentType.LOD0);
                            break;
                        }
                        case 1:
                        {
                            instance.category.Add(CategoryContentType.LOD1);
                            break;
                        }
                        case 2:
                        {
                            instance.category.Add(CategoryContentType.LOD2);
                            break;
                        }
                        case 3:
                        {
                            instance.category.Add(CategoryContentType.LOD3);
                            break;
                        }
                        default:
                        {
                            instance.category.Add(CategoryContentType.LODN);
                            break;
                        }
                    }

                    instance.category |= CategoryContentType.LOD;
                }

                AssetLibraryItem item = AssetLibrary.GetItem(instance.originalTransform.gameObject);
                if (item != null)
                {
                    instance.category |= CategoryContentType.PROP;
                }
            }

            foreach (CategorizedInstance instance in instances)
            {
                CategorizedInstance current = instance.Parent;
                while (current != null)
                {
                    if (current.category.HasBoth(CategoryContentType.PROP))
                    {
                        instance.category.Add(CategoryContentType.IGNORE);
                        break;
                    }

                    current = current.Parent;
                }
            }
        }

        public static void Match(List<CategorizedInstance> originals, List<CategorizedInstance> matches)
        {
            foreach (CategorizedInstance match in matches)
            {
                int index = originals.FindIndex(x =>
                {
                    return x.FullName == match.FullName;
                });

                if (index == Consts.INDEX_NONE)
                {
                    match.category |= CategoryContentType.MISS;
                    continue;
                }

                match.category |= CategoryContentType.MATCH;

                CategorizedInstance original = originals[index];
                if (original.originalTransform != null && match.originalTransform != null && match.originalTransform.parent != null)
                {
                    if (original.Name != match.Name
                     || original.originalTransform.localPosition != match.originalTransform.localPosition
                     || original.originalTransform.localRotation != match.originalTransform.localRotation
                     || original.originalTransform.localScale != match.originalTransform.localScale)
                    {
                        match.category |= CategoryContentType.OVRD;
                    }
                }
            }
        }
        #endregion
    }
}
