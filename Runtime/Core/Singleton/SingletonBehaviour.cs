namespace Prateek.Runtime.Core.Singleton
{
    using UnityEngine;

    //New Rule: 18/11/19
    //All services need to be ServiceSingleton, and can support multiple Provider
    public abstract class SingletonBehaviour<TInstance>
        : MonoBehaviour
        where TInstance : MonoBehaviour
    {
        #region Static and Constants
        private static bool isApplicationQuitting;
        private static TInstance instance;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object INSTANCE_LOCK = new object();
        #endregion

        #region Properties
        public static bool IsApplicationQuitting
        {
            get { return isApplicationQuitting; }
        }

        protected static TInstance Instance
        {
            get
            {
                if (IsApplicationQuitting)
                {
                    return null;
                }

                lock (INSTANCE_LOCK)
                {
                    if (instance == null)
                    {
                        //Specific behavior for abstract ServiceProviders
                        if (typeof(TInstance).IsAbstract)
                        {
                            Debug.Assert(instance != null, $"Instance for {typeof(TInstance).Name} is not setup");
                            return null;
                        }
                        else
                        {
                            new GameObject(typeof(TInstance).Name).AddComponent<TInstance>();
                        }
                    }

                    return instance;
                }
            }
        }

        protected abstract string ParentName { get; }
        #endregion

        #region Unity Methods
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void DomainReload()
        {
            instance = null;
            isApplicationQuitting = false;
        }

        protected virtual void Awake()
        {
            RegisterInstanceFor(this as TInstance);

            if (!IsApplicationQuitting)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            SetParent();

            OnAwake();
        }

        protected virtual void OnApplicationQuit()
        {
            isApplicationQuitting = true;

            Destroy(gameObject);
        }
        #endregion

        #region Service
        protected abstract void OnAwake();
        #endregion

        #region Class Methods
        private void SetParent()
        {
            var parentName = ParentName;
            if (string.IsNullOrEmpty(parentName))
            {
                parentName = "UNNAMED";
            }

            ParentProvider.AddChildToParent(transform, parentName);
        }

        private void RegisterInstanceFor<TSubClass>(TSubClass registeringInstance) where TSubClass : TInstance
        {
            if (GetType() != typeof(TSubClass) && !GetType().IsSubclassOf(typeof(TSubClass)))
            {
                Debug.Assert(false, $"Trying to set a singleton of type {typeof(TSubClass).Name} in {GetType().Name}");
            }

            SingletonBehaviour<TSubClass>.instance = registeringInstance;

            //todo fix NamingHelper.SetSingleton(registeringInstance);
        }

        protected static bool Exists()
        {
#if UNITY_EDITOR
            return instance != null;
#else
            return true;
#endif
        }
        #endregion
    }
}
