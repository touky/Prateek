namespace Mayfair.Core.Code.Utils.Extensions
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public static class TransformExtensions
    {
        #region Class Methods
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void CopyGlobalValues(this Transform transform, Transform other)
        {
            transform.position = other.position;
            transform.rotation = other.rotation;
            transform.localScale = other.localScale;
        }

        public static void CopyLocalValues(this Transform transform, Transform other)
        {
            transform.localPosition = other.localPosition;
            transform.localRotation = other.localRotation;
            transform.localScale = other.localScale;
        }

        public static void ClearAllChildren(this Transform transform)
        {
            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);

#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    child.SetParent(null);
                    Object.Destroy(child.gameObject);
                }
                else
                {
                    child.SetParent(null);
                    Undo.DestroyObjectImmediate(child.gameObject);
                }
#else
                child.SetParent(null);
                GameObject.Destroy(child.gameObject);
#endif //UNITY_EDITOR
            }
        }

        public static void GatherChildrenOrdered(this Transform root, List<Transform> list, bool parentFirst = true)
        {
            list.Add(root);

            //This ensures that each layer of children is added before the next layer of children is
            for (int l = 0; l < list.Count; l++)
            {
                Transform parent = list[l];
                for (int c = 0; c < parent.childCount; c++)
                {
                    Transform child = parent.GetChild(c);
                    list.Add(child);
                }
            }

            if (!parentFirst)
            {
                list.Reverse();
            }
        }

        public static bool HasGivenParent(this Transform transform, Transform parent)
        {
            Transform current = transform.parent;
            while (current != null)
            {
                if (current == parent)
                {
                    return true;
                }

                current = current.parent;
            }

            return false;
        }

        public static int CountParents(this Transform transform, Transform stopAt = null)
        {
            if (transform == null)
            {
                return 0;
            }

            int count = 0;
            Transform parent = transform.parent;
            while (parent != transform && parent != null && (stopAt == null || parent != stopAt))
            {
                count++;

                parent = parent.parent;
            }

            return count;
        }

        public static RectTransform ToRectTransform(this Transform transform)
        {
            Debug.Assert(transform is RectTransform);

            return transform as RectTransform;
        }
        #endregion
    }
}
