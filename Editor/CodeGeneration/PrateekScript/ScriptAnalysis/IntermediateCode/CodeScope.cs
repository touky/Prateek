namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.IntermediateCode
{
    using System.Collections.Generic;

    public class CodeScope : CodeCommand
    {
        #region Fields
        public List<CodeCommand> commands = new List<CodeCommand>();
        public List<CodeScope> scopeContent = new List<CodeScope>();
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
                scopeContent.Add(scope);
            }
            else if (!(other is CodeComment))
            {
                commands.Add(other);
            }
        }
        #endregion
    }
}
