namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    public abstract class DebugMenuObject
    {
        #region Fields
        protected string title;
        protected bool isOpen;
        #endregion

        #region Properties
        public string Title { get { return title; } }

        public bool IsOpen { get { return isOpen; } internal set { isOpen = value; } }
        #endregion

        #region Constructors
        protected DebugMenuObject(string title = "Empty Content")
        {
            this.title = title;
            isOpen = true;
        }
        #endregion

        #region Class Methods
        internal void Draw(DebugMenuContext context)
        {
            OnDraw(context);
        }

        protected abstract void OnDraw(DebugMenuContext context);
        #endregion
    }
}
