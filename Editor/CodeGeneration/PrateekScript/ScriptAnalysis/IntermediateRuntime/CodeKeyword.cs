namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    [DebuggerDisplay("{GetType().Name}>{keyword.Content}")]
    public class CodeKeyword : CodeCommand
    {
        #region Fields
        public IKeyword keyword;
        public List<IKeyword> arguments = new List<IKeyword>();
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

            if (other is CodeScope code)
            {
                if (scopeContent == code && code.Separator != null)
                {
                    foreach (var command in code.innerCommands)
                    {
                        if (command is CodeKeyword argument)
                        {
                            arguments.Add(argument.keyword);
                        }
                    }

                    scopeContent = null;
                }
                else
                {
                    scopeContent = code;
                }
            }
        }

        public void Add(IKeyword argument)
        {
            arguments.Add(argument);
        }
        #endregion
    }
}
