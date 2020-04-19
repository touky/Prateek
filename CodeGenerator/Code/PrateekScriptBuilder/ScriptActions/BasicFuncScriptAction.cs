// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeGeneration;
    using Prateek.Core.Code;

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
        protected override void GatherVariants(List<FunctionVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            variants.Clear();
            if (data.funcInfos.Count == 0)
            {
                variants.Add(new FunctionVariant());
            }
            else
            {
                var variant = new FunctionVariant(string.Empty, data.funcInfos.Count - 1);
                for (var d = 0; d < data.funcInfos.Count; d++)
                {
                    var funcInfo  = data.funcInfos[d];
                    var varsCount = Vars.GetCount(funcInfo.data);
                    for (var v = 0; v < infoSrc.variables.Count; v++)
                    {
                        var funcData = funcInfo.data;
                        for (var n = 0; n < CSharp.min(varsCount, Vars.Count); n++)
                        {
                            var vars = Vars[n] + infoSrc.variables[v];
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
