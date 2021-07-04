namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.IntermediateRuntime;
    using Prateek.Editor.CodeGeneration.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    public class ScriptAnalyzer
    {
        #region Fields
        ///-----------------------------------------------------------------
        public string content;

        ///-----------------------------------------------------------------
        private int position = 0;
        private int lineCount = 0;
        private int trailingWhiteSpaces = 0;
        private List<string> scopes = new List<string>();

        ///-----------------------------------------------------------------
        private Regex endOfLine = new Regex(@"([\r\n$]+)");

        private SymbolRegistry registry;
        private List<Symbol> contentSymbols = new List<Symbol>();
        private CodeScope contentRootScope = null;

        private int cursorSymbol = 0;
        #endregion

        #region Properties
        ///-----------------------------------------------------------------
        public string Scope
        {
            get
            {
                if (scopes.Count == 0)
                {
                    return string.Empty;
                }

                var i = scopes.Count - 1;
                while (i >= 0 && scopes[i] == string.Empty)
                {
                    i++;
                }

                return i < 0 ? string.Empty : scopes[i];
            }
        }

        public CodeScope ContentRootScope
        {
            get { return contentRootScope; }
        }

        private Symbol CurrentSymbol
        {
            get { return cursorSymbol < contentSymbols.Count ? contentSymbols[cursorSymbol] : null; }
        }

        private Symbol NextSymbol
        {
            get
            {
                var cursor = cursorSymbol + 1;
                return cursor < contentSymbols.Count ? contentSymbols[cursor] : null;
            }
        }
        #endregion

        #region Class Methods
        ///--
        public ScriptAnalyzer(SymbolRegistry registry)
        {
            this.registry = registry;
        }

        ///--
        public void Init(string content)
        {
            position = 0;
            lineCount = 0;
            trailingWhiteSpaces = 0;
            scopes.Clear();

            this.content = content;
        }

        ///--
        public void FindAllSymbols()
        {
            contentSymbols.Clear();
            while (position < content.Length)
            {
                var closestMatch  = (Match) null;
                var closestSymbol = (Symbol) null;
                foreach (var symbol in registry.Symbols)
                {
                    var match = symbol.Start.Match(content, position);
                    if (match.Success && (closestMatch == null || match.Index < closestMatch.Index))
                    {
                        closestMatch = match;
                        closestSymbol = symbol;
                    }
                }

                if (closestSymbol == null)
                {
                    break;
                }

                position = closestMatch.Index + closestMatch.Length;
                if (closestSymbol.End != null || closestSymbol.CanStopWithEndOfLine)
                {
                    var length          = 0;
                    var closestEndMatch = (Match) null;
                    if (closestSymbol.End != null)
                    {
                        var matchEnd = closestSymbol.End.Match(content, position);
                        if (matchEnd.Success)
                        {
                            closestEndMatch = matchEnd;
                            length = 0;
                        }
                    }

                    if (closestSymbol.CanStopWithEndOfLine)
                    {
                        var matchEnd = endOfLine.Match(content, position);
                        if (matchEnd.Success && (closestEndMatch == null || matchEnd.Index < closestEndMatch.Index))
                        {
                            closestEndMatch = matchEnd;
                            length = matchEnd.Length;
                        }
                    }

                    if (closestEndMatch != null && closestEndMatch.Success)
                    {
                        var subStringLength = closestEndMatch.Index + length - position;
                        var foundContent    = content.Substring(position, subStringLength);
                        contentSymbols.Add(closestSymbol.Clone(foundContent));
                        position += subStringLength;
                    }
                }
                else
                {
                    contentSymbols.Add(closestSymbol.Clone(closestMatch.Value));
                }
            }
        }

        ///--
        public struct CodeContext
        {
            public CodeComment latestComments;
            public CodeScope activeScope;
            public CodeCommand latestCommand;
        }

        public bool BuildCodeCommands()
        {
            var scopes = new Stack<CodeScope>();

            contentRootScope = new CodeScope();
            var ctx = new CodeContext()
            {
                latestComments = null,
                activeScope = contentRootScope,
                latestCommand = null
            };

#if true
            while (CurrentSymbol != null)
            {
                var currentSymbol = CurrentSymbol;
                if (currentSymbol is ISeparator)
                {
                    if (ctx.latestCommand == null)
                    {
                        return false;
                    }
                }

                if (ctx.latestCommand != null
                     && ctx.latestCommand.CanAbsorb(currentSymbol))
                {
                    ctx.latestCommand.Add(currentSymbol);
                    cursorSymbol++;
                    continue;
                }

                if (ctx.activeScope != null
                    && ctx.activeScope.CanAbsorb(currentSymbol))
                {
                    cursorSymbol++;
                    continue;
                }

                switch (currentSymbol)
                {
                    case ISeparator separator:
                    {
                        //If the separator has not been absorbed, it's considered a failure
                        return false;
                    }
                    case IScopeOpen opener:
                    {
                        if (ctx.latestCommand != null && !ctx.latestCommand.AllowInternalScope)
                        {
                            return false;
                        }

                        var newScope = registry.SymbolToCommand[currentSymbol.GetType()]() as CodeScope;
                        if (newScope == null)
                        {
                            return false;
                        }

                        newScope.opener = opener;
                        newScope.Add(ctx.latestComments);
                        if (ctx.latestCommand != null)
                        {
                            newScope.owner = ctx.latestCommand;
                            ctx.latestCommand.Add(newScope);
                        }
                        else
                        {
                            newScope.owner = ctx.activeScope;
                            ctx.activeScope.Add(newScope);
                        }

                        scopes.Push(ctx.activeScope);
                        ctx.activeScope = newScope;
                        ctx.latestCommand = null;

                        break;
                    }
                    case IScopeClose closer:
                    {
                        if (scopes.Count == 0)
                        {
                            return false;
                        }

                        if (!ctx.activeScope.Match(closer))
                        {
                            return false;
                        }

                        ctx.activeScope.Close();
                        ctx.activeScope = scopes.Pop();
                        ctx.latestComments = new CodeComment();
                        ctx.latestCommand = null;

                        break;
                    }
                    case IComment keywordComment:
                    case LiteralValue literalValue:
                    {
                        var newCommand = registry.SymbolToCommand[currentSymbol.GetType()]() as CodeCommand;
                        if (newCommand == null)
                        {
                            return false;
                        }

                        if (currentSymbol is IComment)
                        {
                            if (ctx.latestComments == null)
                            {
                                ctx.latestComments = newCommand as CodeComment;
                            }
                        }
                        else
                        {
                            ctx.latestCommand = newCommand;
                        }

                        newCommand.Add(currentSymbol);
                        break;
                    }
                    case IKeyword keywordSymbol:
                    {
                        var newKeyword = registry.SymbolToCommand[currentSymbol.GetType()]() as CodeKeyword;
                        if (newKeyword == null)
                        {
                            return false;
                        }

                        newKeyword.keyword = keywordSymbol;
                        newKeyword.Add(ctx.latestComments);
                        ctx.activeScope.Add(newKeyword);

                        ctx.latestCommand = newKeyword;

                        break;
                    }
                    case IClearCommand clearCommand:
                    {
                        ctx.latestCommand = null;

                        break;
                    }
                }

                cursorSymbol++;
            }
#else
            while (CurrentSymbol != null)
            {
                var currentSymbol = CurrentSymbol;
                if (currentSymbol is IComment commentSymbol)
                {
                    ctx.latestComments.Set(commentSymbol);

                    cursorSymbol++;
                    continue;
                }

                if (currentSymbol is ScopeCodeBegin)
                {
                    if (ctx.latestCommand != null && !ctx.latestCommand.AllowInternalScope)
                    {
                        return false;
                    }

                    var newScope = new CodeScope();
                    newScope.Add(ctx.latestComments);
                    if (ctx.latestCommand != null)
                    {
                        ctx.latestCommand.Add(newScope);
                    }
                    else
                    {
                        ctx.activeScope.Add(newScope);
                    }

                    scopes.Push(ctx.activeScope);
                    ctx.activeScope = newScope;
                    ctx.latestCommand = null;
                }
                else if (currentSymbol is ScopeCodeEnd)
                {
                    if (scopes.Count == 0)
                    {
                        return false;
                    }

                    ctx.activeScope = scopes.Pop();
                    ctx.latestComments = new CodeComment();
                    ctx.latestCommand = null;
                }

                if (currentSymbol is Keyword keywordSymbol)
                {
                    var newKeyword = new CodeKeyword {keyword = keywordSymbol};
                    newKeyword.Add(ctx.latestComments);
                    ctx.activeScope.Add(newKeyword);

                    ctx.latestCommand = newKeyword;

                    if (!(NextSymbol is ScopeInvokeBegin))
                    {
                        if (NextSymbol == null)
                        {
                            return false;
                        }

                        cursorSymbol++;
                        continue;
                    }

                    cursorSymbol++;

                    var latestSymbol   = (Symbol) null;
                    var foundInvokeEnd = false;
                    cursorSymbol++;
                    while (CurrentSymbol != null)
                    {
                        currentSymbol = CurrentSymbol;
                        if (currentSymbol is VariableSeparator)
                        {
                            if (!(latestSymbol is Keyword))
                            {
                                return false;
                            }
                        }
                        else if (currentSymbol is Keyword argSymbol)
                        {
                            if (latestSymbol is Keyword)
                            {
                                return false;
                            }

                            newKeyword.Add(argSymbol);
                        }
                        else if (currentSymbol is ScopeInvokeEnd)
                        {
                            foundInvokeEnd = true;
                            break;
                        }
                        else
                        {
                            return false;
                        }

                        latestSymbol = currentSymbol;
                        cursorSymbol++;
                    }

                    if (!foundInvokeEnd)
                    {
                        return false;
                    }
                }

                if (currentSymbol is LiteralValue literalSymbol)
                {
                    var newLiteral = new CodeLiteral();
                    newLiteral.Add(literalSymbol);

                    ctx.activeScope.Add(newLiteral);

                    if (!(NextSymbol is LiteralValue))
                    {
                        if (NextSymbol == null)
                        {
                            return false;
                        }

                        cursorSymbol++;
                        continue;
                    }

                    cursorSymbol++;
                    while (CurrentSymbol != null)
                    {
                        currentSymbol = CurrentSymbol;
                        if (currentSymbol is LiteralValue nextLiteral)
                        {
                            newLiteral.Add(nextLiteral);
                        }

                        if (!(NextSymbol is LiteralValue))
                        {
                            break;
                        }

                        cursorSymbol++;
                    }
                }

                cursorSymbol++;
            }
#endif

            return true;
        }
#endregion
    }
}
