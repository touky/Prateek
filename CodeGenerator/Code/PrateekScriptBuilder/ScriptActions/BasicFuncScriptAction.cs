// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code;

    public partial class BasicFuncScriptAction : ScriptAction
    {
        //---------------------------------------------------------------------
        internal static BasicFuncScriptAction Create(string extension)
        {
            return new BasicFuncScriptAction(extension);
        }

        //-----------------------------------------------------------------
        public override string ScopeTag { get { return "FUNC_BASIC"; } }
        public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }
        public override bool GenerateDefault { get { return false; } }

        //-----------------------------------------------------------------
        public BasicFuncScriptAction(string extension) : base(extension) { }

        //-----------------------------------------------------------------
        #region Rule internal
        protected override void GatherVariants(List<FuncVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            variants.Clear();
            if (data.funcInfos.Count == 0)
            {
                variants.Add(new FuncVariant());
            }
            else
            {
                var variant = new FuncVariant(String.Empty, data.funcInfos.Count - 1);
                for (int d = 0; d < data.funcInfos.Count; d++)
                {
                    var funcInfo  = data.funcInfos[d];
                    var varsCount = Vars.GetCount(funcInfo.data);
                    for (int v = 0; v < infoSrc.variables.Count; v++)
                    {
                        var funcData = funcInfo.data;
                        for (int n = 0; n < CSharp.min(varsCount, Vars.Count); n++)
                        {
                            var vars = (Vars[n] + infoSrc.variables[v]);
                            funcData = vars.Apply(funcData);
                        }

                        variant[d] = funcData;
                    }
                }
                variants.Add(variant);
            }
        }
        #endregion Rule internal
    }
}