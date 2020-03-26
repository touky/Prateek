namespace Assets.Prateek.ToConvert.Service
{
    using System;
    using UnityEngine;

    /// <summary>
    ///     GameInitActivator enables its children one after another to ensure correct activation order
    ///     NOTE: The Activator only works on the first direct layer of children, so grouping objects below one of its child
    ///     will automatically means that all those objects are activated at the same frame.
    /// </summary>
    public class GameInitActivator : MonoBehaviour
    {
        #region Fields
        private int childIndex = 0;
        #endregion

        #region Properties
        public bool ActivationDone
        {
            get { return enabled == false; }
        }
        #endregion

        #region Unity Methods
        public void Update()
        {
            var childIndex = 0;
            try
            {
                for (; childIndex < transform.childCount; childIndex++)
                {
                    //Extremely simple, very efficient
                    var child = transform.GetChild(childIndex);

                    child.gameObject.SetActive(true);
                }

                enabled = false;
            }
            catch (Exception e)
            {
                var child = transform.GetChild(childIndex);

                //DebugTools.LogError($"System loading with {typeof(GameInitActivator).Name} has crashed when initializing {child.name}, game state is wrong, check the log for the error");
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }
}
