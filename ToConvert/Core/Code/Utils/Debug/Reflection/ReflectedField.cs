namespace Mayfair.Core.Code.Utils.Debug.Reflection
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class ReflectedField<T> : IReflectedField
    {
        #region Fields
        private string fieldName;
        private object instance;
        private Type instanceType;
        private FieldInfo fieldInfo;
        #endregion

        #region Properties
        public string FieldName
        {
            get { return fieldName; }
        }

        public T Value
        {
            get { return this; }
            set { Set(value); }
        }
        #endregion

        #region Class Methods
        public static implicit operator ReflectedField<T>(string fieldName)
        {
            return new ReflectedField<T> {fieldName = fieldName};
        }

        public static implicit operator T(ReflectedField<T> ownerField)
        {
            Debug.Assert(ownerField.instance != null);
            Debug.Assert(ownerField.instanceType != null);

            return (T) ownerField.fieldInfo.GetValue(ownerField.instance);
        }

        private void Set(T value)
        {
            Debug.Assert(instance != null);
            Debug.Assert(fieldInfo != null);

            fieldInfo.SetValue(instance, value);
        }

        public bool TryInit(object instance)
        {
            InternalInit(instance);

            return fieldInfo != null && fieldInfo.FieldType == typeof(T);
        }

#if UNITY_EDITOR
        public SerializedProperty GetProperty()
        {
            UnityEngine.Object unityObject = instance as UnityEngine.Object;
            if (unityObject == null)
            {
                return null;
            }

            SerializedObject serializedInstance = new SerializedObject(unityObject);
            if (serializedInstance == null)
            {
                return null;
            }

            return serializedInstance.FindProperty(fieldName);
        }
#endif

        private void InternalInit(object instance)
        {
            this.instance = instance;
            instanceType = instance.GetType();
            Type searchType = instanceType;
            do
            {
                fieldInfo = ReflectionUtils.GetMember(searchType, fieldName);
                searchType = searchType.BaseType;
            } while (fieldInfo == null && searchType != null);
        }
        #endregion

        #region IReflectedField Members
        public void Init(object instance)
        {
            InternalInit(instance);

            Debug.Assert(fieldInfo != null && fieldInfo.FieldType == typeof(T));
        }

        #endregion
    }
}
