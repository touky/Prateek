namespace Mayfair.Core.Editor.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using UnityEditor;

    /// <summary>
    ///     Wrapper class to help with EditorPrefs usage
    /// </summary>
    public class EditorPrefsWrapper
    {
        #region Fields
        private List<string> prefixes = new List<string>();
        private string storageFormat = string.Empty;
        #endregion

        #region Constructors
        #region Constructor
        /// <summary>
        ///     Constructor for the wrapper
        /// </summary>
        /// <param name="prefixes">Prefixes applied to each of the stored prefs to ensure uniqueness of your integration</param>
        public EditorPrefsWrapper(params string[] prefixes)
        {
            this.prefixes.AddRange(prefixes);

            RefreshStorageFormat();
        }
        #endregion
        #endregion

        #region Class Methods
        /// <summary>
        ///     Set a specific prefix value
        /// </summary>
        /// <param name="index">the index in the existing list of prefixes</param>
        /// <param name="value">the new value</param>
        /// <returns>false if the index is invalid</returns>
        public bool SetPrefix(int index, string value)
        {
            if (index < 0 || index >= this.prefixes.Count)
            {
                return false;
            }

            this.prefixes[index] = value;

            RefreshStorageFormat();

            return true;
        }

        /// <summary>
        ///     Clear the key for this type and tag
        /// </summary>
        /// <typeparam name="T">the prefs type</typeparam>
        /// <param name="tag">the prefs tag</param>
        public void ClearKey<T>(string tag)
        {
            string formatTag = FormatTag<T>(tag);
            EditorPrefs.DeleteKey(formatTag);
        }

        private void RefreshStorageFormat()
        {
            this.storageFormat = "{0}_{1}";
            foreach (string prefix in this.prefixes)
            {
                this.storageFormat = string.Format("{0}_{1}", prefix, this.storageFormat);
            }
        }

        private string FormatTag<T>(string tag)
        {
            return string.Format(this.storageFormat, tag, typeof(T).Name);
        }
        #endregion

        #region Enum
        /// <summary>
        ///     Sets an enum value at the tag in the EditorPrefs
        ///     The tag is the Enum Type
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="setValue">The value to set</param>
        public void SetEnum<T>(T setValue) where T : struct, IConvertible
        {
            InternalGetSetEnum(false, typeof(T).Name, ref setValue, setValue);
        }

        /// <summary>
        ///     Gets an enum value at the tag from the EditorPrefs
        ///     The tag is the Enum Type
        ///     If not found, default value is getValue
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="getValue">The value to get</param>
        public void GetEnum<T>(ref T getValue) where T : struct, IConvertible
        {
            InternalGetSetEnum(true, typeof(T).Name, ref getValue, getValue);
        }

        /// <summary>
        ///     Gets an enum value at the tag from the EditorPrefs
        ///     The tag is the Enum Type
        ///     If not found, default value is defaultValue
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="getValue">The value to get</param>
        /// <param name="defaultValue">Default value if not found</param>
        public void GetEnum<T>(ref T getValue, T defaultValue) where T : struct, IConvertible
        {
            InternalGetSetEnum(true, typeof(T).Name, ref getValue, defaultValue);
        }

        /// <summary>
        ///     Sets an enum value at the given tag in the EditorPrefs
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="setValue">The value to set</param>
        public void SetEnum<T>(string tag, T setValue) where T : struct, IConvertible
        {
            InternalGetSetEnum(false, tag, ref setValue, setValue);
        }

        /// <summary>
        ///     Gets an enum value at the given tag from the EditorPrefs
        ///     If not found, default value is defaultValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">The value to get</param>
        public void GetEnum<T>(string tag, ref T getValue) where T : struct, IConvertible
        {
            InternalGetSetEnum(true, tag, ref getValue, getValue);
        }

        /// <summary>
        ///     Gets an enum value at the given tag from the EditorPrefs
        ///     If not found, default value is defaultValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">The value to get</param>
        /// <param name="defaultValue">Default value if not found</param>
        public void GetEnum<T>(string tag, ref T getValue, T defaultValue) where T : struct, IConvertible
        {
            InternalGetSetEnum(true, tag, ref getValue, defaultValue);
        }

        private void InternalGetSetEnum<T>(bool get, string tag, ref T value, T defaultValue) where T : struct, IConvertible
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            int refValue = value.ToInt32(culture);
            int refDef = defaultValue.ToInt32(culture);
            InternalGetSet(get, tag, ref refValue, refDef);
            value = (T) (object) refValue;
        }
        #endregion

        #region boolean
        /// <summary>
        ///     Sets a boolean value at the given tag in the EditorPrefs
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="setValue">the value to store</param>
        public void Set<T>(T tag, bool setValue)
        {
            InternalGetSet(false, tag.ToString(), ref setValue, setValue);
        }

        /// <summary>
        ///     Gets a boolean value at the given tag from the EditorPrefs
        ///     If not found, default value is getValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        public void Get<T>(T tag, ref bool getValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, getValue);
        }

        /// <summary>
        ///     Gets a boolean value at the given tag from the EditorPrefs
        ///     If not found, default value is defaultValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        /// <param name="defaultValue">Default value if not found</param>
        public void Get<T>(T tag, ref bool getValue, bool defaultValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, defaultValue);
        }

        private void InternalGetSet(bool get, string tag, ref bool getValue)
        {
            InternalGetSet(get, tag, ref getValue, getValue);
        }

        private void InternalGetSet(bool get, string tag, ref bool value, bool defaultValue)
        {
            string formatTag = FormatTag<bool>(tag);
            if (get)
            {
                value = EditorPrefs.GetBool(formatTag, defaultValue);
            }
            else
            {
                EditorPrefs.SetBool(formatTag, value);
            }
        }
        #endregion

        #region integer
        /// <summary>
        ///     Sets an int value at the given tag in the EditorPrefs
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="setValue">the value to store</param>
        public void Set<T>(T tag, int setValue)
        {
            InternalGetSet(false, tag.ToString(), ref setValue, setValue);
        }

        /// <summary>
        ///     Gets an int value at the given tag from the EditorPrefs
        ///     If not found, default value is getValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        public void Get<T>(T tag, ref int getValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, getValue);
        }

        /// <summary>
        ///     Gets an int value at the given tag from the EditorPrefs
        ///     If not found, default value is defaultValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        /// <param name="defaultValue">Default value if not found</param>
        public void Get<T>(T tag, ref int getValue, int defaultValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, defaultValue);
        }

        private void InternalGetSet(bool get, string tag, ref int value, int defaultValue)
        {
            string formatTag = FormatTag<int>(tag);
            if (get)
            {
                value = EditorPrefs.GetInt(formatTag, defaultValue);
            }
            else
            {
                EditorPrefs.SetInt(formatTag, value);
            }
        }
        #endregion

        #region float
        /// <summary>
        ///     Sets a float value at the given tag in the EditorPrefs
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="setValue">the value to store</param>
        public void Set<T>(T tag, float setValue)
        {
            InternalGetSet(false, tag.ToString(), ref setValue, setValue);
        }

        /// <summary>
        ///     Gets a float value at the given tag from the EditorPrefs
        ///     If not found, default value is getValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        public void Get<T>(T tag, ref float getValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, getValue);
        }

        /// <summary>
        ///     Gets a float value at the given tag from the EditorPrefs
        ///     If not found, default value is defaultValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        /// <param name="defaultValue">Default value if not found</param>
        public void Get<T>(T tag, ref float getValue, float defaultValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, defaultValue);
        }

        private void InternalGetSet(bool get, string tag, ref float value)
        {
            InternalGetSet(get, tag, ref value, value);
        }

        private void InternalGetSet(bool get, string tag, ref float value, float defaultValue)
        {
            string formatTag = FormatTag<float>(tag);
            if (get)
            {
                value = EditorPrefs.GetFloat(formatTag, defaultValue);
            }
            else
            {
                EditorPrefs.SetFloat(formatTag, value);
            }
        }
        #endregion

        #region string
        /// <summary>
        ///     Sets a string value at the given tag in the EditorPrefs
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="setValue">the value to store</param>
        public void Set<T>(T tag, string setValue)
        {
            InternalGetSet(false, tag.ToString(), ref setValue, setValue);
        }

        /// <summary>
        ///     Gets a string value at the given tag from the EditorPrefs
        ///     If not found, default value is getValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        public void Get<T>(T tag, ref string getValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, getValue);
        }

        /// <summary>
        ///     Gets a string value at the given tag from the EditorPrefs
        ///     If not found, default value is defaultValue
        /// </summary>
        /// <typeparam name="T">the tag type</typeparam>
        /// <param name="tag">the tag</param>
        /// <param name="getValue">the value to store</param>
        /// <param name="defaultValue">Default value if not found</param>
        public void Get<T>(T tag, ref string getValue, string defaultValue)
        {
            InternalGetSet(true, tag.ToString(), ref getValue, defaultValue);
        }

        private void InternalGetSet(bool get, string tag, ref string value)
        {
            InternalGetSet(get, tag, ref value, value);
        }

        private void InternalGetSet(bool get, string tag, ref string value, string defaultValue)
        {
            string formatTag = FormatTag<string>(tag);
            if (get)
            {
                value = EditorPrefs.GetString(formatTag, defaultValue);
            }
            else
            {
                EditorPrefs.SetString(formatTag, value);
            }
        }
        #endregion
    }
}
