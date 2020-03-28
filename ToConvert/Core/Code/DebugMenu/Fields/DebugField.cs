namespace Mayfair.Core.Code.DebugMenu.Fields
{
    public abstract class DebugField
    {
        #region Fields
        private int indent = 0;
        #endregion

        #region Properties
        public int Indent
        {
            get { return this.indent; }
            set { this.indent = value; }
        }

        public int LineCount
        {
            get { return 1; }
        }
        #endregion

        #region Unity EditorOnly Methods
        public abstract void OnGUI(DebugMenuContext context);
        #endregion

        #region Class Methods
        public virtual void Draw(DebugMenuContext context)
        {
            context.Draw(this);
        }
        #endregion
    }
}