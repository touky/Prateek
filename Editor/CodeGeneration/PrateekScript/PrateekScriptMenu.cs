namespace Prateek.CodeGeneration.PrateekScript.Editor
{
    using UnityEditor;
    using UnityEngine;
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using Prateek.Core.Code.Consts;
    using Prateek.Core.Editor.Helpers;

    public static class PrateekScriptMenu
    {
        #region Menu items
        [MenuItem(ConstMenu.CREATE_ASSET + "Prateek Script")]
        public static Object CreatePrateekScript()
        {
            return AssetMenuExtensions.CreateAsset(defaultContent, "prtk");
        }

        private static string defaultContent =
            $@"///---
{Glossary.Macros[FunctionKeyword.FILE_INFO]}(MyExportFile, cs)
{{
    //{Glossary.Macros.codeBlockFormat}CHOSE_A_FUNC(Prateek.MyNamespace, ContainerClass) //, Additional class option
    //{{
    //    //{Glossary.Macros[FunctionKeyword.CLASS_INFO]}(MyClassInfo) {{ {Glossary.Macros[VariableKeyword.VARS]}(var0, var1, ....) }}
    //
    //    {Glossary.Macros[FunctionKeyword.PREFIX]}
    //    {{
    //        @//Prefix code
    //    }}
    //
    //    {Glossary.Macros[FunctionKeyword.MAIN]}
    //    {{
    //        @//Main code, repeated by function
    //    }}
    //    
    //    {Glossary.Macros[FunctionKeyword.SUFFIX]}
    //    {{
    //        @//Suffix code
    //    }}
    //}}
}}
";
        #endregion
    }
}
