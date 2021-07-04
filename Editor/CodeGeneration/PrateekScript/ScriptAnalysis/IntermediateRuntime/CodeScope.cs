namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime
{
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;
    using UnityEditor.Experimental.GraphView;

    public class CodeScope : CodeCommand
    {
        #region Fields
        public IScopeOpen opener;
        public CodeCommand owner;
        public List<CodeCommand> innerCommands = new List<CodeCommand>();
        public List<CodeScope> innerScopes = new List<CodeScope>();
        #endregion

        #region Properties
        public override bool AllowInternalScope
        {
            get { return true; }
        }

        public virtual ISeparator Separator
        {
            get { return null; }
        }
        #endregion

        #region Class Methods
        public override void Add(CodeCommand other)
        {
            base.Add(other);

            if (other is CodeScope scope)
            {
                innerScopes.Add(scope);
            }
            else if (!(other is CodeComment))
            {
                innerCommands.Add(other);
            }
        }

        public bool Match(IScopeClose closer)
        {
            if (opener == null)
            {
                return false;
            }

            return opener.Match(closer);
        }

        public void Close()
        {
            owner.Add(this);
        }
        #endregion
    }
}
