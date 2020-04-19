namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeCommands {
    using System.Collections.Generic;

    public class CodeScope : CodeCommand
    {
        public override bool AllowInternalScope { get { return true; } }

        public List<CodeCommand> commands = new List<CodeCommand>();
        public List<CodeScope> scopeContent = new List<CodeScope>();

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
    }
}