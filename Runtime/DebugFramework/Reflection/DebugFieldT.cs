namespace Prateek.Runtime.DebugFramework.Reflection
{
    using UnityEngine;

    ///----
    /// <summary>
    /// DebugField uses reflection to retrieve a field inside of the given owner.
    /// Usage: var myField = (DebugField<MyType>)(owner, "fieldName");
    /// Notes: Any DebugField declared as field in a DebugMenuSection is automatically init on section Ctor call
    /// </summary>
    public class DebugField<T> : DebugField
    {
        #region Properties
        public T Value { get { return fieldInfo == null ? default(T) : this; } set { Set(value); } }
        #endregion

        #region Class Methods
        public static DebugField<T> operator +(DebugField<T> other, object owner)
        {
            other.SetOwner(owner);
            return other;
        }

        public static implicit operator DebugField<T>(string fieldName)
        {
            var field = new DebugField<T>();
            field.SetName(fieldName);
            return field;
        }

        public static implicit operator DebugField<T>((object, string) pair)
        {
            var field = new DebugField<T>();
            field.SetName(pair.Item2);
            field.SetOwner(pair.Item1);
            return field;
        }

        public static implicit operator T(DebugField<T> other)
        {
            if (!other.IsValid)
            {
                Debug.LogError("Trying to access invalid debug field:\n- Either test IsValid before accessing it\n- Use AssertDrawable in the DebugMenu to show an invalidity warning.");
            }

            return other.fieldInfo == null ? default : (T) other.fieldInfo.GetValue(other.owner);
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
            base.SetOwner(owner);

            if (owner == null || string.IsNullOrEmpty(Name))
            {
                return;
            }

            if (fieldInfo.FieldType != typeof(T))
            {
                Debug.LogError($"Field type for '{Name}' is invalid:\n- Is '{fieldInfo.FieldType}'\n- Expected '{typeof(T)}'");
                fieldInfo = null;
            }
        }
        #endregion
    }
}
