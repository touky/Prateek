namespace Prateek.DaemonCore.Code
{
    using Prateek.TickableFramework.Code.Enums;

    public interface IUpdatable
    {
        #region Unity Application Methods
        void OnApplicationQuit();

        // Application Messages
        void OnApplicationFocus(bool focusStatus);
        void OnApplicationPause(bool pauseStatus);
        #endregion

        #region Unity EditorOnly Methods
        // Ui Messages
        void OnGUI();
        #endregion

        #region Class Methods
        // Object Lifetime Messages
        /// <summary>
        ///     OnInitialize is called when the object is created. OnInitialize is not called during deserialization.
        /// </summary>
        void OnInitialize();

        /// <summary>
        ///     On Start is called at the start of the next frame after the object has been created. OnStart is not called during
        ///     deserialization.
        /// </summary>
        void OnStart();

        /// <summary>
        ///     OnUpdate is called every frame.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnUpdate(TickType tickType, float seconds);

        /// <summary>
        ///     OnTimescaleIndependantUpdate is called every frame, after every OnUpdate has been called but before any
        ///     OnLateUpdate has been called. It's deltaTime is timscale independant.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnUpdateUnscaled(TickType tickType, float seconds);

        /// <summary>
        ///     OnLateUpdate is called every frame after the OnUpdate for every object has been called.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnLateUpdate(TickType tickType, float seconds);

        /// <summary>
        ///     OnFixedUpdate is called every fixed physics engine update.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnFixedUpdate(TickType tickType, float seconds);

        /// <summary>
        ///     OnDispose is called just before the object is destroyed.
        /// </summary>
        void OnDispose();

        /// <summary>
        ///     Called when registering the object for updates. Unlike OnInitialize and OnStart, OnRegister is called during
        ///     serialization. (I couldn't call it OnEnable because it is a Unity message that ScriptableObjects receive)
        /// </summary>
        void OnRegister();

        /// <summary>
        ///     Called when unregistering the object for updates. (I couldn't call it OnDisable because it is a Unity message that
        ///     ScriptableObjects receive)
        /// </summary>
        void OnUnregister();
        #endregion
    }
}
