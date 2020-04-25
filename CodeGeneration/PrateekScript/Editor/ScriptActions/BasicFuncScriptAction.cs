// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using global::Prateek.Core.Code;

    public partial class BasicFuncScriptAction : ScriptAction
    {
        #region Properties
        ///-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "FUNC_BASIC"; }
        }

        public override GenerationMode GenMode
        {
            get { return GenerationMode.ForeachSrc; }
        }

        public override bool GenerateDefault
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public BasicFuncScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        internal static BasicFuncScriptAction Create(string extension)
        {
            return new BasicFuncScriptAction(extension);
        }

        ///-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            variants.Clear();
            if (data.functionContents.Count == 0)
            {
                variants.Add(new FunctionVariant());
            }
            else
            {
                var variant = new FunctionVariant(string.Empty, data.functionContents.Count - 1);
                for (var d = 0; d < data.functionContents.Count; d++)
                {
                    var funcInfo  = data.functionContents[d];
                    var varsCount = Vars.GetCount(funcInfo.data);
                    for (var v = 0; v < contentSrc.variables.Count; v++)
                    {
                        var funcData = funcInfo.data;
                        for (var n = 0; n < CSharp.min(varsCount, Vars.Count); n++)
                        {
                            var vars = Vars[n] + contentSrc.variables[v];
                            funcData = vars.Apply(funcData);
                        }

                        variant[d] = funcData;
                    }
                }

                variants.Add(variant);
            }
        }
        #endregion Rule internal
        #endregion
    }
}
