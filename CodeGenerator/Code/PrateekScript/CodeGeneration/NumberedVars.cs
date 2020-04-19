namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration {
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.CodeGenerator.Code.Utils;
    using global::Prateek.CodeGenerator.PrateekScriptBuilder;
    using global::Prateek.Core.Code.Helpers;

    public struct NumberedVars
    {
        //-------------------------------------------------------------
        private List<string> datas;

        //-------------------------------------------------------------
        public int Count { get { return datas.Count; } }
        public StringSwap this[int i]
        {
            get
            {
                if (i < 0 || i >= datas.Count)
                    return String.Empty;
                return datas[i];
            }
        }

        //-------------------------------------------------------------
        public NumberedVars(global::Assets.Prateek.CodeGenerator.Code.PrateekScript.CodeGeneration.Glossary.Macro.VarName root)
        {
            datas = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                datas.Add(String.Format("{0}_{1}", root, i).Keyword());
            }
        }

        //-------------------------------------------------------------
        public int GetCount(string content)
        {
            if (datas == null || content == null)
                return 0;

            var count = 0;
            for (int c = 0; c < Count; c++)
            {
                count += content.Contains(datas[c]) ? 1 : 0;
            }
            return count;
        }
    }
}