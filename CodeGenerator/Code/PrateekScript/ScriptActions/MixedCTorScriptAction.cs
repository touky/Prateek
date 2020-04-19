// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeGeneration;

    public class MixedCTorScriptAction : ScriptAction
    {
        internal static MixedCTorScriptAction Create(string extension)
        {
            return new MixedCTorScriptAction(extension);
        }

        //-----------------------------------------------------------------
        public override string ScopeTag { get { return "CTOR_MIXED"; } }
        public override GenerationMode GenMode { get { return GenerationMode.ForeachSrc; } }

        //-----------------------------------------------------------------
        public MixedCTorScriptAction(string extension) : base(extension) { }

        //-----------------------------------------------------------------
        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ContentInfos data, ClassInfos infoSrc, ClassInfos infoDst)
        {
            var slots = new int[infoSrc.VarCount];
            for (int s = 0; s < slots.Length; s++)
            {
                slots[s] = 0;
            }

            variants.Clear();
            GatherVariants(0, slots, slots.Length, variants, data, infoSrc);

            //Add Default vec(f)
            var variant = new FunctionVariant(infoSrc.names[0], 2);
            variant[1] = String.Format(Glossary.Code.argsN, data.classDefaultType, 0);
            for (int v = 0; v < infoSrc.VarCount; v++)
            {
                variant[2] = String.Format(Glossary.Code.varsN, 0);
            }
            variants.Add(variant);
        }

        //-----------------------------------------------------------------
        private void GatherVariants(int s, int[] slots, int count, List<FunctionVariant> variants, ContentInfos data, ClassInfos infoSrc)
        {
            var classCount = data.classInfos.Count + 1;
            for (int c = 0; c < classCount; c++)
            {
                var varCount = c == 0 ? 1 : data.classInfos[c - 1].VarCount;
                slots[s] = c;
                if (count - varCount > 0)
                {
                    GatherVariants(s + 1, slots, count - varCount, variants, data, infoSrc);
                }
                else if (count - varCount == 0)
                {
                    var sn      = 0;
                    var sv      = 0;
                    var variant = new FunctionVariant(infoSrc.names[0], 2);
                    for (int v = 0; v < slots.Length && v < s + 1; v++)
                    {
                        var sl = slots[v];
                        if (sl == 0)
                        {
                            variant[1] = String.Format(Glossary.Code.argsN, data.classDefaultType, sn);
                            variant[2] = String.Format(Glossary.Code.varsN, sn);
                            sn++;
                        }
                        else
                        {
                            sl -= 1;
                            var info = data.classInfos[sl];
                            variant[1] = String.Format(Glossary.Code.argsV_, info.className, sv);
                            for (int vr = 0; vr < info.VarCount; vr++)
                            {
                                variant[2] = String.Format(Glossary.Code.varsV_, sv, info.variables[vr]);
                            }
                            sv++;
                        }
                    }

                    variants.Add(variant);
                }
            }
        }
        #endregion Rule internal
    }
}