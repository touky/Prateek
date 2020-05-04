namespace Prateek.CodeGeneration.PrateekScript.Editor
{
    using Prateek.CodeGeneration.Code.PrateekScript.CodeGeneration;
    using UnityEditor;
    using UnityEngine;

    public static class PrateekScriptMenu
    {
        #region Menu items
        //[MenuItem(ConstMenu.CREATE_ASSET + "Prateek Script")]
        //public static Object CreatePrateekScript()
        //{
        //    //    return AssetMenuExtensions.CreateAsset(defaultContent, "prtk");
        //}

        private static string defaultContent =
            $@"///---
{Glossary.Macros[Glossary.FuncName.FILE_INFO]}(MyExportFile, cs)
{{
    //{Glossary.Macros.codeBlockFormat}CHOSE_A_FUNC(Prateek.MyNamespace, ContainerClass) //, Additional class option
    //{{
    //    //{Glossary.Macros[Glossary.FuncName.CLASS_INFO]}(MyClassInfo) {{ {Glossary.Macros[Glossary.VarName.VARS]}(var0, var1, ....) }}
    //
    //    {Glossary.Macros[Glossary.FuncName.PREFIX]}
    //    {{
    //        @//Prefix code
    //    }}
    //
    //    {Glossary.Macros[Glossary.FuncName.MAIN]}
    //    {{
    //        @//Main code, repeated by function
    //    }}
    //    
    //    {Glossary.Macros[Glossary.FuncName.SUFFIX]}
    //    {{
    //        @//Suffix code
    //    }}
    //}}
}}
";
        #endregion
    }
}
