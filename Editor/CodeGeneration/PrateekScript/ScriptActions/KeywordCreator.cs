namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Editor.CodeGeneration.CodeBuilder.Utils;
    using Prateek.Editor.CodeGeneration.PrateekScript.RuntimeGeneration;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.Utils;
    using Prateek.Runtime.Core.Helpers;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static class KeywordCreator
    {
        public static KeywordUsage GetFileInfos()
        {
            return new KeywordUsage(Glossary.Macros[FunctionKeyword.FILE_INFO], string.Empty)
            {
                arguments = 2, needOpenScope = true, keywordUsageType = KeywordUsageType.Ignore
            };
        }

        public static void AddDefine(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.DEFINE], Glossary.Macros[FunctionKeyword.FILE_INFO])
            {
                arguments = 1,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeFile.AddDefine(arguments[0].Content);
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.DEFINE], codeBlock)
            {
                arguments = 1,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.AddDefine(arguments[0].Content);
                    return true;
                }
            });
        }

        public static void AddUsing(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            Func<List<Keyword>, string> feedMethod = (arguments) =>
            {
                var content = string.Empty;
                foreach (var argument in arguments)
                {
                    if (string.IsNullOrEmpty(content))
                    {
                        content += " ";
                    }

                    content += argument.Content;
                }

                return content;
            };

            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.USING], Glossary.Macros[FunctionKeyword.FILE_INFO])
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeFile.AddNamespace(feedMethod(arguments));
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.USING], codeBlock)
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.AddNamespace(feedMethod(arguments));
                    return true;
                }
            });
        }

        public static void AddCodeImport(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.CODE_IMPORT], codeBlock)
            {
                arguments = 1,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    var sourcePath = Path.Combine(fileData.source.directory, arguments[0].Content);
                    if (File.Exists(sourcePath))
                    {
                        var fileBody = File.ReadAllText(sourcePath);
                        fileBody = fileBody.CleanText();
                        fileBody = CodeFile.CleanPrateekComments(fileBody);
                        codeFile.ScriptContent.SetFileBody(fileBody);
                        return true;
                    }
                    return false;
                }
            });
        }

        public static void AddCodeBlock(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(codeBlock, Glossary.Macros[FunctionKeyword.FILE_INFO])
            {
                arguments = 1, needOpenScope = true, createNewScriptContent = true,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.blockNamespace = arguments[0].Content;
                    return true;
                },
                onCloseScope = (codeFile, scope) =>
                {
                    codeFile.Commit();
                    return scope == codeBlock;
                }
            });
        }
        
        public static void AddDefineContainer(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.DEFINE_CONTAINER], codeBlock)
            {
                arguments = 1,
                needOpenScope = true,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.blockClassName = arguments[0].Content;
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.TYPE], Glossary.Macros[FunctionKeyword.DEFINE_CONTAINER])
            {
                arguments = ArgumentRange.Between(1, 2),
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.blockClassPrefix.Add(arguments[0].Content);
                    if (arguments.Count == 2)
                    {
                        codeInfos.blockClassParent = arguments[1].Content;
                    }
                    return true;
                }
            });

            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.ATTRIBUTES], Glossary.Macros[FunctionKeyword.DEFINE_CONTAINER])
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    for (int a = arguments.Count - 1; a >= 0; a--)
                    {
                        codeInfos.blockClassPrefix.Insert(0, arguments[a].Content);
                    }
                    return true;
                }
            });
        }

        public static void AddClassInfo(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.CLASS_INFO], codeBlock)
            {
                arguments = 1,
                needOpenScope = true,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.classInfos.Add(new ClassContent {className = arguments[0].Content});
                    return true;
                }
            });
        }

        public static void AddNames(List<KeywordUsage> keywordUsages)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[VariableKeyword.NAMES], Glossary.Macros[FunctionKeyword.CLASS_INFO])
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    return codeInfos.SetClassNames(arguments);
                }
            });
        }

        public static void AddVars(List<KeywordUsage> keywordUsages)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[VariableKeyword.VARS], Glossary.Macros[FunctionKeyword.CLASS_INFO])
            {
                arguments = ArgumentRange.AtLeast(1),
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    return codeInfos.SetClassVars(arguments);
                }
            });
        }

        public static void AddDefault(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.DEFAULT], codeBlock)
            {
                arguments = ArgumentRange.Between(2, 3),
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.classDefaultType = arguments[0].Content;
                    codeInfos.classDefaultValue = arguments[1].Content;
                    codeInfos.classDefaultExportOnly = (arguments.Count == 2 || arguments[2].Content == "false") ? false : true;
                    return true;
                }
            });
        }

        public static void AddFunc(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.FUNC], codeBlock)
            {
                needOpenScope = true,
                needScopeData = true,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.functionContents.Add(new FunctionContent());
                    codeInfos.SetFuncBody(data);
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });
        }

        public static void AddCodeHeader(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.CODE_PREFIX], codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.codeHeader = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });
        }

        public static void AddCodeBody(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.CODE_MAIN], codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.codeBody = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });
        }

        public static void AddCodeFooter(List<KeywordUsage> keywordUsages, string codeBlock)
        {
            keywordUsages.Add(new KeywordUsage(Glossary.Macros[FunctionKeyword.CODE_SUFFIX], codeBlock)
            {
                arguments = 0, needOpenScope = true, needScopeData = true,
                onFeedCodeFile = (fileData, codeFile, codeInfos, arguments, data) =>
                {
                    codeInfos.codeFooter = data;
                    return true;
                },
                onCloseScope = (codeFile, scope) => { return true; }
            });
        }
    }
}
