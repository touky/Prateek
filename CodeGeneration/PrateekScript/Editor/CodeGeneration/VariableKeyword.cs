namespace Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration
{
    using System;

    ///-------------------------------------------------------------
    public enum VariableKeyword
    {
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
