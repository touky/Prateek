namespace Mayfair.Core.Code.VisualAsset
{
    using JetBrains.Annotations;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Enums;
    using Prateek.Runtime.KeynameFramework.Interfaces;
    using UnityEngine;
    using UnityEngine.Serialization;

    public abstract class VisualKeynameUserInstance : MonoBehaviour, IKeynameUser
    {
        #region Settings
        [SerializeField]
        [FormerlySerializedAs("uniqueId")]
        private KeynameSerializable savedKeyname = string.Empty;
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

        public virtual void SetKeyname(Keyname keyname)
        {
            this.keyname = keyname;

            if (!HasLoadedUniqueId)
            {
                return;
            }

            if (this.keyname.State == KeynameState.Fullname)
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
