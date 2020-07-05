namespace Mayfair.Core.Code.VisualAsset
{
    using JetBrains.Annotations;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using UnityEngine;
    using UnityEngine.Serialization;

    public abstract class VisualKeynameUserInstance : MonoBehaviour, IKeynameUser
    {
        #region Settings
        [SerializeField]
        [FormerlySerializedAs("uniqueId")]
        private SerializableKeyname savedKeyname = string.Empty;
        #endregion

        #region Fields
        protected Keyname keyname;

        protected Transform cachedTransform;
        #endregion

        #region Properties
        protected abstract bool HasLoadedUniqueId { get; }
        #endregion

        #region Unity Methods
        [UsedImplicitly]
        private void Awake()
        {
            keyname = savedKeyname;
            cachedTransform = transform;
        }
        #endregion

        #region IIdentifiable Members
        public Keyname Keyname
        {
            get { return keyname; }
        }

        public bool Equals(IKeynameUser other)
        {
            return Keyname == other.Keyname;
        }

        public virtual void SetUniqueId(Keyname keyname)
        {
            this.keyname = keyname;

            if (!HasLoadedUniqueId)
            {
                return;
            }

            if (this.keyname.Type == KeynameState.Fullname)
            {
                NotifyNewId();
            }
            else
            {
                ClearVisual();
            }
        }
        #endregion

        #region IIdentifiable Methods
        protected abstract void NotifyNewId();
        protected abstract void ClearVisual();
        #endregion IIdentifiable Methods
    }
}
