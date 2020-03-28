namespace Mayfair.Core.Editor.AssetLibrary
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using UnityEngine;

    [Serializable]
    public class AssetLibraryItem
    {
        #region Settings
        [SerializeField]
        private bool enabled = true;

        [SerializeField]
        private string name = string.Empty;

        [Space]
        [SerializeField]
        private GameObject replacement = null;

        [SerializeField]
        [HideInInspector]
        private List<string> tags = new List<string>();
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public GameObject Replacement
        {
            get { return replacement; }
        }

        public List<string> Tags
        {
            get
            {
                if (replacement == null)
                {
                    return null;
                }

                if (name != replacement.name)
                {
                    name = replacement.name;
                    RefreshTags();
                }

                return tags;
            }
        }
        #endregion

        #region Constructors
        public AssetLibraryItem(GameObject replacement)
        {
            name = replacement.name;
            this.replacement = replacement;

            RefreshTags();
        }
        #endregion

        #region Class Methods
        public bool RefreshTags()
        {
            List<string> matches = new List<string>();
            RegexHelper.TryFetchingMatches(replacement.name, RegexHelper.AssetTag, matches);

            bool needDirty = tags.Count != matches.Count;
            if (!needDirty)
            {
                for (int t = 0; t < tags.Count; t++)
                {
                    if (tags[t] != matches[t])
                    {
                        needDirty = true;
                        break;
                    }
                }
            }

            if (needDirty)
            {
                tags.Clear();
                tags.AddRange(matches);
                return true;
            }

            return false;
        }

#if UNITY_EDITOR
        public void MarkInvalid()
        {
            replacement = null;
            enabled = false;
        }

        public void SetReplacement(GameObject replacement)
        {
            this.replacement = replacement;
            enabled = true;
            RefreshTags();
        }
#endif
        #endregion
    }
}
