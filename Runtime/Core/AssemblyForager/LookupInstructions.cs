namespace Prateek.Runtime.Core.AssemblyForager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.Core.Helpers.Files;
    using UnityEditor;
    using UnityEngine;

    public class LookupInstructions
    {
        #region Static and Constants
        private static readonly string[] EDITOR_GUIDE = {".Editor", "-Editor"};
        #endregion

        #region Settings
        [SerializeField]
        protected List<string> editorLookup = new List<string>();

        [SerializeField]
        protected List<string> ingameLookup = new List<string>();

        [SerializeField]
        protected List<string> ignoredLookup = new List<string>();
        #endregion

        #region Fields
        private bool isEditor = false;
        private HashSet<string> editor = new HashSet<string>();
        private HashSet<string> ingame = new HashSet<string>();
        private HashSet<string> ignored = new HashSet<string>();
        #endregion

        #region Constructors
        public LookupInstructions() { }

        private LookupInstructions(LookupInstructions other)
        {
            if (other != null)
            {
                editorLookup.AddRange(other.editorLookup);
                ingameLookup.AddRange(other.ingameLookup);
                ignoredLookup.AddRange(other.ignoredLookup);
            }
        }
        #endregion

        #region Class Methods
        public static LookupInstructions Load(string resourcePath, Func<LookupInstructions> getDefault = null)
        {
            var isEditor = Application.isEditor && !Application.isPlaying;

            var assetPath = isEditor
                ? Path.Combine(ConstFolder.ASSETS_RESOURCES, resourcePath)
                : resourcePath;
            var textAsset = isEditor
                ? AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath)
                : Resources.Load<TextAsset>(Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath)));

            var instructions = (LookupInstructions) null;
            if (textAsset == null)
            {
                instructions = new LookupInstructions(getDefault.SafeInvoke());
            }
            else
            {
                instructions = JsonUtility.FromJson<LookupInstructions>(textAsset.text);
            }

            instructions.Init(isEditor);
            return instructions;
        }

        public void Save(string resourcePath)
        {
#if UNITY_EDITOR
            var assetPath = Path.Combine(ConstFolder.ASSETS_RESOURCES, resourcePath);

            Sort();

            //todo trello
            FileHelper.Write(this, assetPath);
            if (!EditorApplication.isPlaying)
            {
                AssetDatabase.Refresh();
            }
#else
            throw new NotImplementedException($"{nameof(LookupInstructions)}: Save() is not supported outside of the editor");
#endif
        }

        public bool Allow(string value, bool checkBoth = false)
        {
            var lookup = isEditor ? editor : ingame;
            var other  = !isEditor ? editor : ingame;
            if (lookup.Contains(value) || (checkBoth && other.Contains(value)))
            {
                return true;
            }
#if UNITY_EDITOR
            else if (!ignored.Contains(value) && !other.Contains(value))
            {
                var storeInEditor = value.Contains(EDITOR_GUIDE);
                var storage       = storeInEditor ? editorLookup : ingameLookup;

                lookup = storeInEditor ? editor : ingame;
                if (lookup.Contains(value))
                {
                    return storeInEditor == isEditor;
                }

                lookup.Add(value);
                storage.Add(value);
            }
#endif

            return false;
        }

        private void Init(bool isEditor)
        {
            this.isEditor = isEditor;

            Init(editorLookup, editor);
            Init(ingameLookup, ingame);
            Init(ignoredLookup, ignored);
        }

        private void Sort()
        {
            editorLookup.Sort();
            ingameLookup.Sort();
            ignoredLookup.Sort();
        }

        private void Init(List<string> list, HashSet<string> hashSet)
        {
            hashSet.Clear();
            foreach (var value in list)
            {
                if (hashSet.Contains(value))
                {
                    continue;
                }

                hashSet.Add(value);
            }
        }
        #endregion
    }
}
