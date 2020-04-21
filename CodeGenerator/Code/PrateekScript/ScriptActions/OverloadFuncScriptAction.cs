// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration;

    public class OverloadFuncScriptAction : ScriptAction
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
        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            variants.Clear();
            var slots = new int[contentSrc.NameCount / 2];
            GatherVariants(0, slots, variants, data, contentSrc, contentDst);
        }

        ///-----------------------------------------------------------------
        private void GatherVariants(int s, int[] slots, List<FunctionVariant> variants, ScriptContent data, ClassContent contentSrc, ClassContent contentDst)
        {
            if (s < slots.Length)
            {
                for (var p = 0; p < 2; p++)
                {
                    slots[s] = p;
                    GatherVariants(s + 1, slots, variants, data, contentSrc, contentDst);
                }
            }
            else
            {
                var variant = new FunctionVariant(string.Empty, 1);
                for (var sv = 0; sv < slots.Length; sv++)
                {
                    if (slots[sv] == 0)
                    {
                        for (var i = 0; i < data.functionContents.Count; i++)
                        {
                            var info = data.functionContents[i].data;
                            info = (Names[0] + contentSrc.names[sv * 2 + 0]).Apply(info);
                            info = (Names[1] + contentSrc.names[sv * 2 + 1]).Apply(info);
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
