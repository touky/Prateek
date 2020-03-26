namespace Assets.Prateek.ToConvert.DebugMenu.Pages
{
    using Assets.Prateek.ToConvert.DebugMenu.Content;

    /// <summary>
    ///     Main class for debug menu pages that need debug from inside of a system
    /// </summary>
    /// <typeparam name="T">IDebugMenuNotebookOwner needs to be in your owner system</typeparam>
    public abstract class DebugMenuPage<T> : DebugMenuPage where T : IDebugMenuNotebookOwner
    {
        #region Fields
        protected T owner;
        #endregion

        #region Constructors
        protected DebugMenuPage(T owner, string title) : base(title)
        {
            this.owner = owner;

            ReflectionUtils.InitAllReflectedFields(this, this.owner);
        }
        #endregion

        #region Class Methods
        public override void Draw(DebugMenuContext context)
        {
            base.Draw(context);

            Draw(owner, context);
        }

        protected abstract void Draw(T owner, DebugMenuContext context);
        #endregion
    }
}
