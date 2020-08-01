namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.IntermediateCode
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    [DebuggerDisplay("{GetType().Name}>{keyword.Content}")]
    public class CodeKeyword : CodeCommand
    {
        #region Fields
        public Keyword keyword;
        public List<Keyword> arguments = new List<Keyword>();
        public CodeScope scopeContent;
        #endregion

        #region Properties
        public override bool AllowInternalScope
        {
            get { return true; }
        }
        #endregion

        #region Class Methods
        public override void Add(CodeCommand other)
        {
            base.Add(other);

            if (other is CodeScope scope)
            {
                scopeContent = scope;
            }
        }

        public void Add(Keyword argument)
        {
            arguments.Add(argument);
        }
        #endregion
    }
}
