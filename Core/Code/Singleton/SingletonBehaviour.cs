namespace Prateek.DaemonCore.Code
{
    using UnityEngine;

    //New Rule: 18/11/19
    //All services need to be ServiceSingleton, and can support multiple Provider
    public abstract class SingletonBehaviour<TInstance> : MonoBehaviour
        where TInstance : MonoBehaviour
    {
        #region Static and Constants
        private static bool applicationIsQuitting;
        private static TInstance instance;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object INSTANCE_LOCK = new object();
        #endregion

        #region Properties
        public static bool ApplicationIsQuitting
        {
            get { return applicationIsQuitting; }
        }

        protected static TInstance Instance
        {
            get
            {
                if (ApplicationIsQuitting)
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
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            RegisterInstanceFor(this as TInstance);

            if (!ApplicationIsQuitting)
            {
                DontDestroyOnLoad(gameObject);
            }

            OnAwake();
        }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;

            Destroy(gameObject);
        }
        #endregion

        #region Service
        protected abstract void OnAwake();
        #endregion

        #region Class Methods
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
