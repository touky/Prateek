namespace Prateek.Runtime.DebugFramework.DebugMenu.Sections
{
    using System;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.DebugFramework.Reflection;

    public abstract class DebugMenuSection
        : DebugMenuObject
    {
        #region Fields
        private DebugMenu.IDebugMenuOwner owner;
        #endregion

        #region Properties
        public virtual Setting Settings { get { return Setting.Nothing; } }
        #endregion

        #region Constructors
        protected DebugMenuSection(string title)
            : base(title) { }
        #endregion

        #region Class Methods
        internal void SetOwner(DebugMenu.IDebugMenuOwner owner)
        {
            if (this.owner == null)
            {
                this.owner = owner;

                DebugField.SetOwnerToAllDebugFields(this, owner);

                ManualInit();
            }
        }

        protected TOwner GetOwner<TOwner>()
            where TOwner : class, DebugMenu.IDebugMenuOwner
        {
            return owner as TOwner;
        }

        protected virtual void ManualInit()
        {
        }
        #endregion

        [Flags]
        public enum Setting
        {
            Nothing = 0,

            AddSeparatorBefore = 1 << 0,
            AddSeparatorAfter = 1 << 1,
        }
    }
}
