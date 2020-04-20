// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;

    public class MixedCTorScriptAction : ScriptAction
    {
        #region Properties
        //-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "CTOR_MIXED"; }
        }

        public override GenerationMode GenMode
        {
            get { return GenerationMode.ForeachSrc; }
        }
        #endregion

        #region Constructors
        //-----------------------------------------------------------------
        public MixedCTorScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        internal static MixedCTorScriptAction Create(string extension)
        {
            return new MixedCTorScriptAction(extension);
        }
        #endregion

        //-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            var slots = new int[contentSrc.VarCount];
            for (var s = 0; s < slots.Length; s++)
            {
                slots[s] = 0;
            }

            variants.Clear();
            GatherVariants(0, slots, slots.Length, variants, data, contentSrc);

            //Add Default vec(f)
            var variant = new FunctionVariant(contentSrc.names[0], 2);
            variant[1] = string.Format(Glossary.Code.argsN, data.classDefaultType, 0);
            for (var v = 0; v < contentSrc.VarCount; v++)
            {
                variant[2] = string.Format(Glossary.Code.varsN, 0);
            }

            variants.Add(variant);
        }

        //-----------------------------------------------------------------
        private void GatherVariants(int s, int[] slots, int count, List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc)
        {
            var classCount = data.classInfos.Count + 1;
            for (var c = 0; c < classCount; c++)
            {
                var varCount = c == 0 ? 1 : data.classInfos[c - 1].VarCount;
                slots[s] = c;
                if (count - varCount > 0)
                {
                    GatherVariants(s + 1, slots, count - varCount, variants, data, contentSrc);
                }
                else if (count - varCount == 0)
                {
                    var sn      = 0;
                    var sv      = 0;
                    var variant = new FunctionVariant(contentSrc.names[0], 2);
                    for (var v = 0; v < slots.Length && v < s + 1; v++)
                    {
                        var sl = slots[v];
                        if (sl == 0)
                        {
                            variant[1] = string.Format(Glossary.Code.argsN, data.classDefaultType, sn);
                            variant[2] = string.Format(Glossary.Code.varsN, sn);
                            sn++;
                        }
                        else
                        {
                            sl -= 1;
                            var info = data.classInfos[sl];
                            variant[1] = string.Format(Glossary.Code.argsV_, info.className, sv);
                            for (var vr = 0; vr < info.VarCount; vr++)
                            {
                                variant[2] = string.Format(Glossary.Code.varsV_, sv, info.variables[vr]);
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
