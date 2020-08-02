namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    public abstract class SyntaxScriptAction : ScriptAction
    {
        #region Constructors
        public SyntaxScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        public abstract void AddKeyword(string content);
        public abstract void AddIdentifier(string content);
        #endregion
    }
}
