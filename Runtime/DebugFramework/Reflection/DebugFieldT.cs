namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Reflection;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class DebugField<T> : DebugField
    {
        #region Fields
        private string name;
        private object owner;
        private Type ownerType;
        private SerializedObject serializedOwner;
        private FieldInfo fieldInfo;
        #endregion

        #region Properties
        public string Name { get { return name; } }

        public bool IsValid { get { return fieldInfo != null; } }

        public T Value { get { return fieldInfo == null ? default(T) : this; } set { Set(value); } }

#if UNITY_EDITOR
        public SerializedProperty SerializedProperty
        {
            get
            {
                if (!(owner is UnityEngine.Object unityOwner))
                {
                    return null;
                }

                if (serializedOwner == null)
                {
                    serializedOwner = new SerializedObject(unityOwner);
                }

                return serializedOwner.FindProperty(name);
            }
        }
#endif
        #endregion

        #region Class Methods
        public static implicit operator DebugField<T>(string fieldName)
        {
            return new DebugField<T> {name = fieldName};
        }

        public static implicit operator T(DebugField<T> other)
        {
            return other.Value;
        }

        private void Set(T value)
        {
            if (fieldInfo == null)
            {
                return;
            }

            fieldInfo.SetValue(owner, value);
        }

        public override void SetOwner(object owner)
        {
            this.owner = owner;
            ownerType = this.owner.GetType();
            fieldInfo = ReflectionHelper.SearchFieldInfo(ownerType, name, true);
        }
        #endregion
    }
}
