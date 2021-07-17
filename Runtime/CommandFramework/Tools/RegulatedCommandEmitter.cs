namespace Prateek.Runtime.CommandFramework.Tools
{
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Gadgets;
    using UnityEngine;

    public abstract class RegulatedCommandEmitter<TCommand, TCommandEmitter>
        where TCommand : Command, new()
        where TCommandEmitter : CommandTools.IEmitter
    {
        #region Static and Constants
        private const float DEFAULT_COOLDOWN = 0.1f;
        #endregion

        #region Fields
        private double cooldown;
        private double realtimeMark;
        protected TCommand cachedCommand;
        protected TCommandEmitter emitter;
        #endregion

        #region Properties
        public TCommand CachedCommand { get { return cachedCommand; } }
        #endregion

        #region Constructors
        protected RegulatedCommandEmitter(TCommandEmitter emitter, double cooldown = DEFAULT_COOLDOWN)
        {
            this.cooldown = cooldown;
            realtimeMark = Time.realtimeSinceStartup;
            this.emitter = emitter;

            MarkTime();
        }
        #endregion

        #region Class Methods
        public void Create()
        {
            if (cachedCommand == null)
            {
                cachedCommand = Command.Create<TCommand>();
            }
        }

        public bool TrySend()
        {
            if (Time.realtimeSinceStartup - realtimeMark < cooldown)
            {
                return false;
            }

            if (cachedCommand == null)
            {
                cachedCommand = Command.Create<TCommand>();
            }

            DoSend();

            cachedCommand = null;
            MarkTime();

            return true;
        }

        private void MarkTime()
        {
            realtimeMark = Time.realtimeSinceStartup;
        }

        protected abstract void DoSend();
        #endregion
    }
}
