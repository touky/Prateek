namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeCommands {
    using System.Collections.Generic;
    using System.Diagnostics;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols;
    using global::Prateek.CodeGenerator.PrateekScriptBuilder;

    [DebuggerDisplay("{GetType().Name}>{keyword.Content}")]
    public class CodeKeyword : CodeCommand
    {
        public override bool AllowInternalScope { get { return true; } }

        public Keyword keyword;
        public List<Keyword> arguments = new List<Keyword>();
        public CodeScope scopeContent;

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
    }
}