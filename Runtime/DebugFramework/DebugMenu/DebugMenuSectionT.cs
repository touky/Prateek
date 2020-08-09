namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using Prateek.Runtime.DebugFramework.Reflection;

    public abstract class DebugMenuSection<TFieldSource> : DebugMenuSection
    {
        #region Constructors
        protected DebugMenuSection(TFieldSource source, string title) : base(title)
        {
            DebugField.SetOwnerToAllDebugFields(this, source);
        }
        #endregion
    }
}
