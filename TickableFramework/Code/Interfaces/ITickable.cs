namespace Prateek.TickableFramework.Code.Interfaces
{
    using Mayfair.Core.Code.Utils.Types.Priority;
    using Prateek.TickableFramework.Code.Enums;

    public interface ITickable : IPriority
    {
        #region Properties
        /// <summary>
        ///     Implement this to tell what tick event this class implements
        /// </summary>
        TickType TickType { get; }
        #endregion

        #region Unity Methods
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
        ///     OnFixedUpdate is called every fixed physics engine update.
        /// </summary>
        /// <param name="frameEvent"></param>
        /// <param name="seconds"></param>
        void FixedUpdate(FrameEvent frameEvent, float seconds);

        /// <summary>
        ///     OnUpdate is called every frame.
        /// </summary>
        /// <param name="frameEvent"></param>
        /// <param name="seconds"></param>
        /// <param name="unscaledSeconds"></param>
        void Update(FrameEvent frameEvent, float seconds, float unscaledSeconds);

        /// <summary>
        ///     OnLateUpdate is called every frame after the OnUpdate for every object has been called.
        /// </summary>
        /// <param name="frameEvent"></param>
        /// <param name="seconds"></param>
        void LateUpdate(FrameEvent frameEvent, float seconds);
        #endregion

        #region Unity Application Methods
        // Application Messages
        void OnApplicationQuit();
        void OnApplicationFocus(bool appStatus);
        void OnApplicationPause(bool appStatus);
        #endregion

        #region Class Methods
        // Ui Messages
        void DrawGUI();
        #endregion
    }
}
