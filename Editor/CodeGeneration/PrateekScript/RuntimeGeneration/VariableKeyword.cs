namespace Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration
{
    using System;

    ///-------------------------------------------------------------
    public enum VariableKeyword
    {
        DEF, //DEF_[n]
        NAMES, //NAMES_[n]
        VARS, //VARS_[n]
        FUNC_RESULT, //FUNC_RESULT_[n]

        MAX
    }

    ///-------------------------------------------------------------
    public static class VariableKeywordExtensions
    {
        public static string S(this VariableKeyword value)
        {
            return Enum.GetNames(typeof(VariableKeyword))[(int) value];
        }
    }
}
