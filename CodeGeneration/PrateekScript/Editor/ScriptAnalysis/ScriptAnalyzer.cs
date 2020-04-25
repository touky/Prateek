namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.IntermediateCode;
    using Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptAnalysis.SyntaxSymbols;

    internal class ScriptAnalyzer
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
        ///-----------------------------------------------------------------
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
                foreach (var symbol in SymbolRegistry.Symbols)
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
        public bool BuildCodeCommands()
        {
            var scopes = new Stack<CodeScope>();

            var latestComments = new CodeComment();
            var activeScope    = contentRootScope = new CodeScope();
            var latestCommand  = (CodeCommand) null;

            while (CurrentSymbol != null)
            {
                var currentSymbol = CurrentSymbol;
                if (currentSymbol is IComment commentSymbol)
                {
                    latestComments.Set(commentSymbol);

                    cursorSymbol++;
                    continue;
                }

                if (currentSymbol is ScopeCodeBegin)
                {
                    if (latestCommand != null && !latestCommand.AllowInternalScope)
                    {
                        return false;
                    }

                    var newScope = new CodeScope();
                    newScope.Add(latestComments);
                    if (latestCommand != null)
                    {
                        latestCommand.Add(newScope);
                    }
                    else
                    {
                        activeScope.Add(newScope);
                    }

                    scopes.Push(activeScope);
                    activeScope = newScope;
                    latestCommand = null;
                }
                else if (currentSymbol is ScopeCodeEnd)
                {
                    if (scopes.Count == 0)
                    {
                        return false;
                    }

                    activeScope = scopes.Pop();
                    latestComments = new CodeComment();
                    latestCommand = null;
                }

                if (currentSymbol is Keyword keywordSymbol)
                {
                    var newKeyword = new CodeKeyword {keyword = keywordSymbol};
                    newKeyword.Add(latestComments);
                    activeScope.Add(newKeyword);

                    latestCommand = newKeyword;

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

                    activeScope.Add(newLiteral);

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

            return true;
        }
        #endregion
    }
}
