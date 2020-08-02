namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;
    using Prateek.Editor.CodeGeneration.CodeBuilder.Utils;
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using Prateek.Runtime.Core.Helpers;

    /// <summary>
    /// Lists all the class-infos and create a new block of code by simply applying the names and vars to said code.
    /// </summary>
    public class RepeatScriptAction : ScriptAction
    {
        #region Properties
        ///-----------------------------------------------------------------
        public override string ScopeTag
        {
            get { return "REPEAT"; }
        }

        public override GenerationRule GenerationMode
        {
            get { return GenerationRule.ForeachSrc; }
        }

        public override bool GenerateDefault
        {
            get { return false; }
        }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public RepeatScriptAction(string extension) : base(extension) { }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        internal static RepeatScriptAction Create(string extension)
        {
            return new RepeatScriptAction(extension);
        }

        private int repeatCount = 0;

        ///-----------------------------------------------------------------

        #region Rule internal
        public override BuildResult Generate(ScriptContent scriptContent)
        {
            repeatCount = int.Parse(scriptContent.classDefaultValue);

            var classContent = scriptContent.classInfos[0];
            var variants     = new List<FunctionVariant>();
            GatherVariants(variants, scriptContent, classContent, classContent);

            for (int r = 1; r <= repeatCount; r++)
            {
                var fileBody = scriptContent.fileBody;
                for (int f = 0; f < Functions.Count; f++)
                {
                    if (!Functions[f].CanSwap(fileBody))
                    {
                        continue;
                    }

                    var swap = Functions[f];
                    var functionContent = scriptContent.functionContents[f];
                    if (Names.DefaultSymbol.CanSwap(functionContent.body))
                    {
                        for (int fv = 0; fv < r; fv++)
                        {
                            if (fv > 0)
                            {
                                swap += Glossary.Macros.codeDataTabsTag;
                            }
                            swap += variants[fv][f];
                        }
                    }
                    else
                    {
                        swap.Replacement = variants[r - 1][f];
                    }

                    var leadingTabulations = fileBody.GetTabulation(swap.IndexOf(fileBody));
                    var swapTabs = (StringSwap) Glossary.Macros.codeDataTabsTag + leadingTabulations;
                    fileBody = swap.Apply(fileBody);
                    fileBody = swapTabs.Apply(fileBody);
                }

                if (Defaults[1].CanSwap(fileBody))
                {
                    var swap = Defaults[1] + r.ToString();
                    fileBody = swap.Apply(fileBody);
                }

                scriptContent.codeGenerated.Add(new ScriptContent.GeneratedCode {className = classContent.className + r.ToString(), code = fileBody});
            }

            return BuildResult.ValueType.Success;
        }

        protected override void GatherVariants(List<FunctionVariant> variants, ScriptContent scriptContent, ClassContent contentSrc, ClassContent contentDst)
        {
            variants.Clear();

            for (int r = 0; r < repeatCount; r++)
            {
                var variant = new FunctionVariant(scriptContent.functionContents.Count);
                for (var fc = 0; fc < scriptContent.functionContents.Count; fc++)
                {
                    var functionContent = scriptContent.functionContents[fc];
                    for (var v = 0; v < contentSrc.names.Count; v++)
                    {
                        var functionBody = functionContent.body;

                        if (Defaults[0].CanSwap(functionBody))
                        {
                            var swap = Defaults[0] + scriptContent.classDefaultType;
                            functionBody = swap.Apply(functionBody);
                        }

                        if (Defaults[1].CanSwap(functionBody))
                        {
                            var swap = Defaults[1] + (r + 1).ToString();
                            functionBody = swap.Apply(functionBody);
                        }

                        if (Names.DefaultSymbol.CanSwap(functionBody))
                        {
                            var swap = Names.DefaultSymbol + contentSrc.names[v] + r.ToString();
                            functionBody = swap.Apply(functionBody);
                        }

                        if (Variables.DefaultSymbol.CanSwap(functionBody))
                        {
                            var swap = Variables.DefaultSymbol + r.ToString();
                            functionBody = swap.Apply(functionBody);
                        }

                        variant[fc] = functionBody;
                    }
                }

                variants.Add(variant);
            }
        }
        #endregion Rule internal
        #endregion
    }
}
