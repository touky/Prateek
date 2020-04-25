// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;

    public partial class SwizzleScriptAction : ScriptAction
    {
        #region Properties
        ///-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "SWIZZLE"; }
        }

        public override GenerationMode GenMode
        {
            get { return GenerationMode.ForeachSrcXDest; }
        }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public SwizzleScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        public static SwizzleScriptAction Create(string extension)
        {
            return new SwizzleScriptAction(extension);
        }
        #endregion

        ///-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            var slots = new int[contentDst.VarCount];
            for (var s = 0; s < slots.Length; s++)
            {
                slots[s] = 0;
            }

            variants.Clear();
            GatherVariantsSlots(0, slots, variants, data, contentSrc, contentDst);
        }

        ///-----------------------------------------------------------------
        private void GatherVariantsSlots(int s, int[] slots, List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            var varCount = contentSrc.VarCount + 1;
            for (var c = 0; c < varCount; c++)
            {
                slots[s] = c;
                if (s + 1 < slots.Length)
                {
                    GatherVariantsSlots(s + 1, slots, variants, data, contentSrc, contentDst);
                }
                else
                {
                    if (data.classDefaultExportOnly)
                    {
                        var foundValid = false;
                        for (var exp = 0; exp < slots.Length; exp++)
                        {
                            if (slots[exp] >= contentSrc.VarCount)
                            {
                                foundValid = true;
                                break;
                            }
                        }

                        if (!foundValid)
                        {
                            continue;
                        }
                    }

                    var sn      = 0;
                    var variant = new FunctionVariant(string.Empty, 2);
                    variant[1] += Glossary.Code.argsV;
                    for (var v = 0; v < slots.Length; v++)
                    {
                        var sv = slots[v];
                        if (sv < contentSrc.VarCount)
                        {
                            var name = sv < contentSrc.NameCount ? contentSrc.names[sv] : contentSrc.variables[sv];
                            variant.Call = name;
                            var variable = contentSrc.variables[sv];
                            variant[2] = string.Format(Glossary.Code.varsV, variable);
                        }
                        else
                        {
                            variant.Call = Glossary.Code.callN;
                            variant[1] = string.Format(Glossary.Code.argsNOpt, data.classDefaultType, sn, data.classDefaultValue);
                            variant[2] = string.Format(Glossary.Code.varsN, sn);
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
