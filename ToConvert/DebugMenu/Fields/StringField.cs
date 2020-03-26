namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    /// <summary>
    ///     Root type for any field drawing a string
    /// </summary>
    public abstract class StringField : DebugField
    {
        #region Fields
        protected string text;
        #endregion

        #region Properties
        public string Text
        {
            get { return text; }
        }
        #endregion

        #region Constructors
        protected StringField() { }

        protected StringField(string text)
        {
            this.text = text;
        }
        #endregion

        #region Class Methods
        public void Draw(DebugMenuContext context, string value)
        {
            text = value;

            Draw(context);
        }
        #endregion
    }
}
