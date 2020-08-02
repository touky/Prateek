namespace Prateek.Editor.CodeGeneration.PrateekScript
{
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using Prateek.Editor.Core.Helpers;
    using Prateek.Runtime.Core.Consts;
    using UnityEditor;
    using UnityEngine;

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
