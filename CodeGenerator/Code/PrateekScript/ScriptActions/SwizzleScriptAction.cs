// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeGeneration;

    public partial class SwizzleScriptAction : ScriptAction
    {
        public static SwizzleScriptAction Create(string extension)
        {
            return new SwizzleScriptAction(extension);
        }
        //-----------------------------------------------------------------
        public override string ScopeTag { get { return "SWIZZLE"; } }
        public override GenerationMode GenMode { get { return GenerationMode.ForeachSrcXDest; } }

        //-----------------------------------------------------------------
        public SwizzleScriptAction(string extension) : base(extension) { }

        //-----------------------------------------------------------------
        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            var slots = new int[infoDst.VarCount];
            for (int s = 0; s < slots.Length; s++)
            {
                slots[s] = 0;
            }

            variants.Clear();
            GatherVariantsSlots(0, slots, variants, data, infoSrc, infoDst);
        }

        //-----------------------------------------------------------------
        private void GatherVariantsSlots(int s, int[] slots, List<FunctionVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            var varCount = infoSrc.VarCount + 1;
            for (int c = 0; c < varCount; c++)
            {
                slots[s] = c;
                if (s + 1 < slots.Length)
                {
                    GatherVariantsSlots(s + 1, slots, variants, data, infoSrc, infoDst);
                }
                else
                {
                    if (data.classDefaultExportOnly)
                    {
                        bool foundValid = false;
                        for (int exp = 0; exp < slots.Length; exp++)
                        {
                            if (slots[exp] >= infoSrc.VarCount)
                            {
                                foundValid = true;
                                break;
                            }
                        }

                        if (!foundValid)
                            continue;
                    }

                    var sn      = 0;
                    var variant = new FunctionVariant(String.Empty, 2);
                    variant[1] += PrateekScriptBuilder.Tag.Code.argsV;
                    for (int v = 0; v < slots.Length; v++)
                    {
                        var sv = slots[v];
                        if (sv < infoSrc.VarCount)
                        {
                            var name = sv < infoSrc.NameCount ? infoSrc.names[sv] : infoSrc.variables[sv];
                            variant.Call = name;
                            var variable = infoSrc.variables[sv];
                            variant[2] = String.Format(PrateekScriptBuilder.Tag.Code.varsV, variable);
                        }
                        else
                        {
                            variant.Call = PrateekScriptBuilder.Tag.Code.callN;
                            variant[1] = String.Format(PrateekScriptBuilder.Tag.Code.argsNOpt, data.classDefaultType, sn, data.classDefaultValue);
                            variant[2] = String.Format(PrateekScriptBuilder.Tag.Code.varsN, sn);
                            sn++;
                        }
                    }

                    if (sn != slots.Length)
                    {
                        variants.Add(variant);
                    }
                }
            }
        }
        #endregion Rule internal
    }
}