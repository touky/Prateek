namespace Mayfair.Core.Editor.Animation
{
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    internal class AnimationHelperContext
    {
        #region Fields
        public AnimationClip target;
        public List<AnimationBindingsSetup> clipBindings = new List<AnimationBindingsSetup>();
        public Dictionary<string, string> bindingPaths = new Dictionary<string, string>();
        public bool bindingModified = false;
        public bool applyModifications = false;
        #endregion

        #region Constructors
        public AnimationHelperContext(AnimationClip target)
        {
            this.target = target;
        }
        #endregion

        #region Class Methods
        public void Init()
        {
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(target);
            foreach (EditorCurveBinding binding in bindings)
            {
                Add(binding, AnimationUtility.GetEditorCurve(target, binding));
            }
        }

        public void Add(EditorCurveBinding binding, AnimationCurve curve)
        {
            AnimationBindingsSetup setup = new AnimationBindingsSetup
            {
                binding = binding,
                curve = curve
            };

            setup.splitPath.AddRange(binding.path.Split(Path.AltDirectorySeparatorChar));
            string builtPath = string.Empty;
            foreach (string path in setup.splitPath)
            {
                if (builtPath != string.Empty)
                {
                    builtPath += Path.AltDirectorySeparatorChar;
                }

                builtPath += path;
                if (!bindingPaths.ContainsKey(builtPath))
                {
                    bindingPaths.Add(builtPath, path);
                }
            }

            clipBindings.Add(setup);
        }
        
        public void ApplyPathChanges()
        {
            List<string> keys = new List<string>(bindingPaths.Keys);
            keys.Sort();

            List<string> oldKeys = new List<string>(bindingPaths.Keys);
            List<KeyPair> conversions = new List<KeyPair>();

            //Store new path
            CreateConversionKeyPair(keys, conversions);

            //So we need to update the propertyPath of the curve (which is written like this "First/Second/Other/MyProp").
            //All conversion values are sorted properly, meaning that the "root folder" is always first, and I also know that no key before 'i' needs updating.
            //Every entry starts at the same root, so if I conversion[i] has changed, I need to update all conversion[i + 1 -> conversion.Count - 1].
            conversions.Sort((a, b) => { return string.Compare(a.key, b.key); });
            for (int i = 0; i < conversions.Count; i++)
            {
                KeyPair root = conversions[i];
                root.value = ConvertPath(root.newKey, root.value);
                for (int j = i + 1; j < conversions.Count; j++)
                {
                    KeyPair converted = conversions[j];
                    if (converted.key.StartsWith(root.key))
                    {
                        converted.newKey = converted.newKey.Replace(root.key, root.value);
                    }

                    conversions[j] = converted;
                }

                conversions[i] = root;
            }

            InsertConversionsIntoBindingPaths(conversions);

            //Rebuild curve bindings
            RebuildCurveBindings();

            bindingPaths.Clear();
            EditorUtility.SetDirty(target);
        }

        private void CreateConversionKeyPair(List<string> keys, List<KeyPair> conversions)
        {
            foreach (string key in keys)
            {
                string value = bindingPaths[key];
                conversions.Add(new KeyPair
                {
                    key = key,
                    newKey = key,
                    value = value
                });
            }
        }

        private void InsertConversionsIntoBindingPaths(List<KeyPair> conversions)
        {
            bindingPaths.Clear();
            foreach (KeyPair conversion in conversions)
            {
                if (string.IsNullOrEmpty(conversion.key))
                {
                    continue;
                }

                bindingPaths[conversion.key] = conversion.value;
            }
        }

        private void RebuildCurveBindings()
        {
            target.ClearCurves();
            foreach (AnimationBindingsSetup binding in clipBindings)
            {
                string newPath = bindingPaths[binding.binding.path].Trim(Path.AltDirectorySeparatorChar);
                target.SetCurve(newPath, binding.binding.type, binding.binding.propertyName, binding.curve);
            }
        }

        public static string ConvertPath(string oldPath, string newEndPath)
        {
            string newPath = TrimPath(oldPath);
            if (!string.IsNullOrEmpty(newPath))
            {
                newPath += Path.AltDirectorySeparatorChar;
            }

            newPath += newEndPath;
            newPath = PathHelper.Simplify(newPath);
            return newPath;
        }

        public static string TrimPath(string oldPath)
        {
            int index = oldPath.LastIndexOf(Path.AltDirectorySeparatorChar);
            string newPath = string.Empty;
            if (index > Consts.INDEX_NONE)
            {
                newPath = oldPath.Substring(0, index);
            }

            return newPath;
        }
        #endregion

        #region Nested type: KeyPair
        private struct KeyPair
        {
            public string key;
            public string newKey;
            public string value;
        }
        #endregion
    }
}
