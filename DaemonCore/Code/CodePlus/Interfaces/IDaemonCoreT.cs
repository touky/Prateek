namespace Prateek.DaemonCore.Code.Interfaces
{
    using Mayfair.Core.Code.Utils.Types.Priority;

    public interface IDaemonCore<TDaemonBranch> : IDaemonCore
        where TDaemonBranch : IDaemonBranch
    {
        #region Registering
        void Register(TDaemonBranch branch);
        void Unregister(TDaemonBranch branch);
        #endregion
    }

    public interface ITickable : IPriority
    {
        #region Class Methods
        /// <summary>
        ///     OnInitialize is called when the object is created. OnInitialize is not called during deserialization.
        /// </summary>
        void Awake();

        /// <summary>
        ///     On Start is called at the start of the next frame after the object has been created. OnStart is not called during
        ///     deserialization.
        /// </summary>
        void Start();

        /// <summary>
        ///     OnUpdate is called every frame.
        /// </summary>
        /// <param name="frameEvent"></param>
        /// <param name="seconds"></param>
        void Update(FrameEvent frameEvent, float seconds, float UnscaledSeconds);

        /// <summary>
        ///     OnLateUpdate is called every frame after the OnUpdate for every object has been called.
        /// </summary>
        /// <param name="frameEvent"></param>
        /// <param name="seconds"></param>
        void LateUpdate(FrameEvent frameEvent, float seconds);

        /// <summary>
        ///     OnFixedUpdate is called every fixed physics engine update.
        /// </summary>
        /// <param name="frameEvent"></param>
        /// <param name="seconds"></param>
        void FixedUpdate(FrameEvent frameEvent, float seconds);
        #endregion
        
        // Application Messages
        void OnApplicationFocus(bool appStatus);
        void OnApplicationPause(bool appStatus);
        void OnApplicationQuit();

        // Ui Messages
        void DrawGUI();
    }

}
