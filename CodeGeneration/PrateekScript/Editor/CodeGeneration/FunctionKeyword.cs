namespace Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration
{
    using System;

    ///-------------------------------------------------------------
    public enum FunctionKeyword
    {
        FILE_INFO,
        DEFINE,
        USING,
        BLOCK,
        PREFIX,
        MAIN,
        SUFFIX,
        CLASS_INFO,
        DEFAULT,
        FUNC,

        MAX
    }

    ///-------------------------------------------------------------
    public static class FunctionKeywordExtensions
    {
        public static string S(this FunctionKeyword value)
        {
            return Enum.GetNames(typeof(FunctionKeyword))[(int) value];
        }
    }
}
