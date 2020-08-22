namespace Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration
{
    using System;

    ///-------------------------------------------------------------
    public enum FunctionKeyword
    {
        FILE_INFO,
        DEFINE,
        DEFINE_CONTAINER,
        TYPE,
        ATTRIBUTES,
        USING,
        CODE_IMPORT,
        BLOCK,
        CODE_PREFIX,
        CODE_MAIN,
        CODE_SUFFIX,
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
