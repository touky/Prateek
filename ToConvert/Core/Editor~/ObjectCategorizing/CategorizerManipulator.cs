namespace Mayfair.Core.Editor.ObjectCategorizing
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils.Extensions;

    public static class CategorizerManipulator
    {
        #region Class Methods
        public static void GatherLodBranches(List<CategorizedInstance> originals, List<CategorizedInstance> merged, CategoryContentType category = CategoryContentType.LOD, bool testEitherCategory = false)
        {
            GatherBranches(originals, merged, category, testEitherCategory);
        }

        public static void GatherBranches(List<CategorizedInstance> originals, List<CategorizedInstance> merged, CategoryContentType category, bool testEitherCategory = false)
        {
            merged.Clear();

            //Find LODs
            foreach (CategorizedInstance original in originals)
            {
                if (testEitherCategory ? !original.category.HasEither(category) : !original.category.HasBoth(category))
                {
                    continue;
                }

                merged.Add(new CategorizedInstance
                {
                    originalTransform = original.originalTransform,
                    category = original.category
                });

                //Gather LOD children
                foreach (CategorizedInstance children in originals)
                {
                    if (!children.originalTransform.HasGivenParent(original.originalTransform))
                    {
                        continue;
                    }

                    merged.Add(new CategorizedInstance
                    {
                        originalTransform = children.originalTransform,
                        category = children.category,
                        Parent = merged.Find(x => { return children.Parent.originalTransform == x.originalTransform; })
                    });
                }
            }
        }

        public static void GatherLodRoot(List<CategorizedInstance> originals, List<CategorizedInstance> roots, CategoryContentType category = CategoryContentType.LOD)
        {
            //Find LODs
            foreach (CategorizedInstance original in originals)
            {
                if (!original.category.HasBoth(category))
                {
                    continue;
                }

                roots.Add(new CategorizedInstance
                {
                    originalTransform = original.originalTransform,
                    category = original.category,
                    Parent = original.Parent
                });
            }

            roots.Sort((a, b) => { return string.Compare(a.Name, b.Name); });
        }

        public static void GatherMergedLods(List<CategorizedInstance> originals, List<CategorizedInstance> merged)
        {
            merged.Clear();

            CategorizedInstance group = "Lod_Group";
            merged.Add(group);

            List<CategorizedInstance> roots = new List<CategorizedInstance>();
            GatherLodRoot(originals, roots);

            foreach (CategorizedInstance root in roots)
            {
                root.Parent = group;
            }

            merged.AddRange(roots);
        }

        public static void GatherPropsRoot(List<CategorizedInstance> originals, List<CategorizedInstance> props)
        {
            props.Clear();

            //Find LODs
            foreach (CategorizedInstance original in originals)
            {
                if (!original.category.HasBoth(CategoryContentType.PROP) || original.category.HasBoth(CategoryContentType.IGNORE))
                {
                    continue;
                }

                props.Add(new CategorizedInstance
                {
                    originalTransform = original.originalTransform,
                    category = original.category
                });
            }
        }

        public static void Reparent(List<CategorizedInstance> children, CategorizedInstance parent)
        {
            foreach (CategorizedInstance child in children)
            {
                child.Parent = parent;
            }
        }
        #endregion
    }
}
