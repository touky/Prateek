namespace Prateek.Runtime.TickableFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.TickableFramework.Enums;

    public interface ITickable : IPriority
    {
        #region Properties
        /// <summary>
        ///     Implement this to tell what tick event this class implements
        /// </summary>
        TickableSetup TickableSetup { get; }
        #endregion

        #region Unity Methods
        /// <summary>
        ///     On Start is called at the start of the next frame after the object has been created.
        ///     OnStart is not called during deserialization.
        /// </summary>
        void InitializeTickable();

        /// <summary>
        ///     OnFixedUpdate is called every fixed physics engine update.
        /// </summary>
        /// <param name="tickableFrame"></param>
        /// <param name="seconds"></param>
        void TickFixed(TickableFrame tickableFrame, float seconds);

        /// <summary>
        ///     OnUpdate is called every frame.
        /// </summary>
        /// <param name="tickableFrame"></param>
        /// <param name="seconds"></param>
        /// <param name="unscaledSeconds"></param>
        void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds);

        /// <summary>
        ///     OnLateUpdate is called every frame after the OnUpdate for every object has been called.
        /// </summary>
        /// <param name="tickableFrame"></param>
        /// <param name="seconds"></param>
        void TickLate(TickableFrame tickableFrame, float seconds);
        #endregion

        #region Unity Application Methods
        // Application Messages
        void ApplicationIsQuitting();
        void ApplicationChangeFocus(bool appStatus);
        void ApplicationChangePause(bool appStatus);
        #endregion

        #region Class Methods
        // Ui Messages
        void DrawGUI();
        #endregion
    }
}
