namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime
{
    using System.Collections.Generic;
    using Mono.Cecil.Cil;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    public abstract class CodeCommand
    {
        #region Fields
        protected List<IComment> comments = new List<IComment>();
        #endregion

        #region Properties
        public virtual bool AllowInternalScope
        {
            get { return false; }
        }
        #endregion

        #region Class Methods
        public virtual bool CanAbsorb(Symbol symbol)
        {
            return false;
        }

        public virtual void Add(CodeCommand other)
        {
            if (other.comments.Count > 0)
            {
                comments.AddRange(other.comments);
                other.comments.Clear();
            }
        }

        public virtual void Add(Symbol symbol)
        {
        }
        #endregion
    }
}
