// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Prateek.CodeGenerator.PrateekScriptBuilder
{
    using System.Collections.Generic;

    public partial class OverloadFuncScriptAction : ScriptAction
    {
        #region Properties
        ///-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "FUNC_OVERLOAD"; }
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
        public OverloadFuncScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        public static OverloadFuncScriptAction Create(string extension)
        {
            return new OverloadFuncScriptAction(extension);
        }
        #endregion

        ///-----------------------------------------------------------------

        #region Rule internal
        protected override void GatherVariants(List<FuncVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            variants.Clear();
            var slots = new int[infoSrc.NameCount / 2];
            GatherVariants(0, slots, variants, data, infoSrc, infoDst);
        }

        ///-----------------------------------------------------------------
        private void GatherVariants(int s, int[] slots, List<FuncVariant> variants, PrateekScriptBuilder.CodeFile.ContentInfos data, PrateekScriptBuilder.CodeFile.ClassInfos infoSrc, PrateekScriptBuilder.CodeFile.ClassInfos infoDst)
        {
            if (s < slots.Length)
            {
                for (var p = 0; p < 2; p++)
                {
                    slots[s] = p;
                    GatherVariants(s + 1, slots, variants, data, infoSrc, infoDst);
                }
            }
            else
            {
                var variant = new FuncVariant(string.Empty, 1);
                for (var sv = 0; sv < slots.Length; sv++)
                {
                    if (slots[sv] == 0)
                    {
                        for (var i = 0; i < data.funcInfos.Count; i++)
                        {
                            var info = data.funcInfos[i].data;
                            info = (Names[0] + infoSrc.names[sv * 2 + 0]).Apply(info);
                            info = (Names[1] + infoSrc.names[sv * 2 + 1]).Apply(info);
                            variant[i] = info;
                        }
                    }
                }

                if (variant.Call != string.Empty)
                {
                    variants.Add(variant);
                }
            }
        }
        #endregion Rule internal
    }
}
