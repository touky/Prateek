namespace Prateek.Runtime
{
    using System.Collections.Generic;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables;
    using Prateek.Runtime.CommandFramework.Servants;
    using UnityEngine;

    public class PrateekBoostrapper : MonoBehaviour
    {
        #region Fields
        protected List<Initializer> bootstrappedServants = new List<Initializer>
        {
            new Initializer<LocalCommandBoostrap>(),
            new Initializer<AddressableRegistryBoostrap>(),
#if UNITY_STANDALONE

            //todo add local one: typeof(AddressableRegistryBoostrap),
#endif
        };

        private int index = 0;
        #endregion

        #region Unity Methods
        protected virtual void Start()
        {
            if (bootstrappedServants.Count == 0)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (index < 0 || index >= bootstrappedServants.Count)
            {
                enabled = false;
                return;
            }

            var initializer = bootstrappedServants[index++];
            var gameObject  = new GameObject(initializer.Name);
            gameObject.transform.SetParent(transform);
            initializer.Init(gameObject);
        }
        #endregion

        #region Nested type: Initializer
        protected abstract class Initializer
        {
            #region Properties
            public abstract string Name { get; }
            #endregion

            #region Class Methods
            public abstract void Init(GameObject gameObject);
            #endregion
        }

        private class Initializer<T>
            : Initializer
            where T : Component
        {
            #region Properties
            public override string Name { get { return typeof(T).Name; } }
            #endregion

            #region Class Methods
            public override void Init(GameObject gameObject)
            {
                gameObject.AddComponent<T>();
            }
            #endregion
        }
        #endregion
    }
}
