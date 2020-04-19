namespace Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer {
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Symbols;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeAnalyzer.Utils;
    using Assets.Prateek.CodeGenerator.Code.PrateekScriptBuilder.CodeCommands;
    using global::Prateek.Core.Code;
    using global::Prateek.Core.Code.Extensions;

    internal class ScriptAnalyzer
    {
        //-----------------------------------------------------------------

        //-----------------------------------------------------------------
        public string content;
        public bool allowKeywordStartWithAlpha = false;

        //-----------------------------------------------------------------
        private int position = 0;
        private int lineCount = 0;
        private int trailingWhiteSpaces = 0;
        private List<string> scopes = new List<string>();

        //-----------------------------------------------------------------
        private char[] charAllow = new char[] { '_', '.' };
        private char[] charLiteral = new char[] { '@', '$' };
        private char[] charIgnore = new char[2] { ' ', '\n' };
        private char[] charCalls = new char[2] { '(', ')' };
        private char[] charArgs = new char[1] { ',' };
        private char[] charScope = new char[2] { '{', '}' };
        private char[] charComment = new char[2] { '/', '*' };

        //-----------------------------------------------------------------
        public bool ShouldContinue { get { return position < content.Length; } }
        public string Scope
        {
            get
            {
                if (scopes.Count == 0)
                    return String.Empty;

                int i = scopes.Count - 1;
                while (i >= 0 && scopes[i] == String.Empty)
                {
                    i++;
                }
                return i < 0 ? String.Empty : scopes[i];
            }
        }

        //-----------------------------------------------------------------
        public void Init(string content)
        {
            position = 0;
            lineCount = 0;
            trailingWhiteSpaces = 0;
            scopes.Clear();

            this.content = content;

            //RemoveComments();
        }

        private Regex endOfLine = new Regex($@"([\r\n$]+)");
        private List<Symbol> registry = new List<Symbol>();
        private List<Symbol> contentSymbols = new List<Symbol>();
        private CodeScope contentRootScope = null;
        public CodeScope ContentRootScope {
            get { return contentRootScope;}
        }

        public void FindAllSymbols()
        {
            if (registry.Count == 0)
            {
                registry.Add(new CommentMultiline());
                registry.Add(new CommentSingleLine());
                registry.Add(new LiteralValue());
                registry.Add(new ScopeCodeBegin());
                registry.Add(new ScopeCodeEnd());
                registry.Add(new ScopeInvokeBegin());
                registry.Add(new ScopeInvokeEnd());
                registry.Add(new VariableSeparator());
                registry.Add(new Keyword());
                registry.Add(new Value());
            }

            contentSymbols.Clear();
            while (position < content.Length)
            {
                var closestMatch  = (Match) null;
                var closestSymbol = (Symbol) null;
                foreach (var symbol in registry)
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
                        var subStringLength = (closestEndMatch.Index + length) - position;
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

        private int cursorSymbol = 0;

        private Symbol CurrentSymbol { get { return cursorSymbol < contentSymbols.Count ? contentSymbols[cursorSymbol] : null; } }

        private Symbol NextSymbol
        {
            get
            {
                var cursor = cursorSymbol + 1;
                return cursor < contentSymbols.Count ? contentSymbols[cursor] : null;
            }
        }

        public bool BuildCodeCommands()
        {
            var scopes = new Stack<CodeScope>();

            var latestComments = new CodeComment();
            var activeScope    = (contentRootScope = new CodeScope());
            var latestCommand  = (CodeCommand)null;

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
                    var newKeyword = new CodeKeyword() {keyword = keywordSymbol};
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

                    var latestSymbol   = (Symbol)null;
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

        //-----------------------------------------------------------------
        public global::Prateek.CodeGenerator.CodeBuilder.BuildResult FindKeyword(ref string keyword)
        {
            keyword = String.Empty;
            while (position < content.Length)
            {
                var type = GetSymbol(content[position]);
                switch (type)
                {
                    case SymbolType.Numeric:
                    case SymbolType.Letter:
                    {
                        if (type == SymbolType.Numeric && keyword.Length == 0)
                            return (global::Prateek.CodeGenerator.CodeBuilder.BuildResult)global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.PrateekScriptKeywordCannotStartWithNumeric + String.Format("at line: {0}", lineCount);
                        keyword += content[position];
                        break;
                    }
                    case SymbolType.Comment:
                    case SymbolType.ScopeStart:
                    case SymbolType.WhiteSpace:
                    case SymbolType.LineFeed:
                    {
                        if (type == SymbolType.LineFeed)
                            lineCount++;

                        if (keyword.Length > 0)
                            return global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.Success;

                        if (type == SymbolType.ScopeStart)
                            scopes.Add(String.Empty);
                        break;
                    }
                    case SymbolType.ScopeEnd:
                    {
                        return global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.Ignored;
                    }
                    default:
                    {
                        if (keyword.Length > 0)
                            return global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.Success;
                        return (global::Prateek.CodeGenerator.CodeBuilder.BuildResult)global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.PrateekScriptWrongKeywordChar + content[position].ToString() + String.Format("at line: {0}", lineCount);
                    }
                }

                position++;
            }
            return keyword.Length > 0 ? global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.Success : global::Prateek.CodeGenerator.CodeBuilder.BuildResult.ValueType.Ignored;
        }

        //-----------------------------------------------------------------
        public bool FindArgs(List<string> args, KeywordUsage keywordUsage)
        {
            args.Clear();

            var  argSplit      = SymbolType.MAX;
            var  argScope      = SymbolType.MAX;
            var  foundScope    = false;
            var  keyword       = String.Empty;
            bool allowContinue = true;
            while (allowContinue && position < content.Length)
            {
                var type = GetSymbol(content[position]);
                switch (type)
                {
                    case SymbolType.CallStart:
                    {
                        if (keywordUsage.arguments.NoneNeeded)
                            return false;

                        if (argScope != SymbolType.MAX)
                            return false;

                        argSplit = SymbolType.ArgSplit;
                        argScope = SymbolType.CallStart;
                        break;
                    }
                    case SymbolType.CallEnd:
                    {
                        if (keywordUsage.arguments.NoneNeeded)
                            return false;

                        if (argScope != SymbolType.CallStart)
                            return false;

                        argSplit = SymbolType.ArgSplit;
                        argScope = SymbolType.CallEnd;

                        allowContinue = keywordUsage.needOpenScope;
                        if (keyword.Length > 0)
                        {
                            args.Add(keyword);
                            keyword = String.Empty;
                        }
                        break;
                    }
                    case SymbolType.Numeric:
                    case SymbolType.Letter:
                    {
                        if (keywordUsage.arguments.NoneNeeded)
                            return false;

                        keyword += content[position];
                        argSplit = SymbolType.Letter;
                        break;
                    }
                    case SymbolType.WhiteSpace:
                    case SymbolType.LineFeed:
                    case SymbolType.ArgSplit:
                    {
                        if (type == SymbolType.LineFeed)
                            lineCount++;

                        if (type == SymbolType.ArgSplit)
                        {
                            if (keywordUsage.arguments.NoneNeeded)
                                return false;

                            if (argSplit == SymbolType.ArgSplit)
                                return false;
                        }

                        argSplit = type;
                        if (keyword.Length > 0)
                        {
                            args.Add(keyword);
                            keyword = String.Empty;
                        }
                        break;
                    }
                    case SymbolType.ScopeStart:
                    {
                        scopes.Add(keywordUsage.needOpenScope ? keywordUsage.keyword : String.Empty);

                        foundScope = true;
                        allowContinue = false;
                        if (!keywordUsage.needOpenScope)
                            break;

                        if (argScope != SymbolType.CallEnd && argSplit == SymbolType.ArgSplit)
                            return false;

                        if (argScope != SymbolType.CallEnd && argScope != SymbolType.MAX)
                            return false;
                        break;
                    }
                    default:
                    {
                        return false;
                    }
                }

                position++;
            }

            if (keywordUsage.needOpenScope && !foundScope)
                return false;
            return keywordUsage.arguments.Check(args.Count);
        }

        //-----------------------------------------------------------------
        public bool FindData(ref string data, KeywordUsage setup)
        {
            if (!setup.needScopeData)
                return true;
            data = String.Empty;

            var allowContinue = true;
            var storeData     = false;
            var scopeCount    = 1;
            while (allowContinue && position < content.Length)
            {
                var type = GetSymbol(content[position]);
                switch (type)
                {
                    case SymbolType.ScopeStart:
                    {
                        if (storeData)
                        {
                            data += content[position];
                            break;
                        }

                        scopeCount++;
                        break;
                    }
                    case SymbolType.ScopeEnd:
                    {
                        if (storeData)
                        {
                            data += content[position];
                            break;
                        }

                        if (--scopeCount == 0)
                        {
                            return true;
                        }
                        break;
                    }
                    case SymbolType.Literal:
                    {
                        storeData = true;
                        break;
                    }
                    case SymbolType.LiteralEnd:
                    case SymbolType.LineFeed:
                    {
                        if (type == SymbolType.LineFeed)
                            lineCount++;

                        if (storeData)
                        {
                            if (type != SymbolType.LiteralEnd)
                                data += content[position];
                            storeData = false;
                        }
                        break;
                    }
                    default:
                    {
                        if (storeData)
                        {
                            data += content[position];
                        }
                        break;
                    }
                }

                position++;
            }
            return true;
        }

        //-----------------------------------------------------------------
        public bool FindScopeEnd(ref string scopeName)
        {
            while (position < content.Length)
            {
                var type = GetSymbol(content[position]);
                switch (type)
                {
                    case SymbolType.ScopeEnd:
                    {
                        if (scopes.Count == 0)
                            return false;

                        scopeName = scopes.Last();
                        scopes.RemoveLast();
                        position++;
                        return true;
                    }
                    case SymbolType.WhiteSpace:
                    {
                        trailingWhiteSpaces++;
                        break;
                    }
                    case SymbolType.LineFeed:
                    {
                        if (type == SymbolType.LineFeed)
                            lineCount++;

                        trailingWhiteSpaces = 0;
                        break;
                    }
                    default:
                    {
                        return false;
                    }
                }

                position++;
            }
            return false;
        }

        //-----------------------------------------------------------------
        private void RemoveComments()
        {
            int  commentStart          = -1;
            int  commentEnd            = -1;
            bool ignoreAllToLiteralEnd = false;
            bool lookForLineEnd        = false;
            bool lookForContEnd        = false;
            for (int i = 0; i < content.Length; i++)
            {
                var value0 = content[i];
                var value1 = content[CSharp.min(i + 1, content.Length - 1)];
                if (ignoreAllToLiteralEnd)
                {
                    if (value0 == charIgnore[1] || value0 == charLiteral[1])
                    {
                        ignoreAllToLiteralEnd = false;
                    }
                    continue;
                }

                if (lookForLineEnd || lookForContEnd)
                {
                    if (lookForLineEnd && value0 == charIgnore[1])
                        commentEnd = i;
                    if (lookForContEnd && value0 == charComment[1] && value1 == charComment[0])
                        commentEnd = ++i;
                    if (i + 1 >= content.Length - 1)
                        commentEnd = i;

                    if (commentEnd >= 0)
                    {
                        content = content.Remove(commentStart, (commentEnd + 1) - commentStart);
                        i = commentStart - 1;
                        lookForLineEnd = false;
                        lookForContEnd = false;
                        commentStart = -1;
                        commentEnd = -1;
                    }

                    continue;
                }

                if (value0 == charComment[0])
                {
                    lookForLineEnd = value1 == charComment[0];
                    lookForContEnd = value1 == charComment[1];

                    if (lookForLineEnd || lookForContEnd)
                        commentStart = i;
                }
                else if (value0 == charLiteral[0])
                {
                    ignoreAllToLiteralEnd = true;
                }
            }
        }

        //-----------------------------------------------------------------
        private SymbolType GetSymbol(char value)
        {
            if (value == charIgnore[1])
            {
                trailingWhiteSpaces = 0;
                return SymbolType.LineFeed;
            }

            if (value == charIgnore[0])
            {
                trailingWhiteSpaces++;
                return SymbolType.WhiteSpace;
            }

            if (value == charCalls[0])
                return SymbolType.CallStart;

            if (value == charCalls[1])
                return SymbolType.CallEnd;

            if (value == charScope[0])
            {
                return SymbolType.ScopeStart;
            }

            if (value == charScope[1])
            {
                return SymbolType.ScopeEnd;
            }

            if (value == charLiteral[0])
            {
                return SymbolType.Literal;
            }

            if (value == charLiteral[1])
            {
                return SymbolType.LiteralEnd;
            }

            for (int c = 0; c < charArgs.Length; c++)
            {
                if (charArgs[c] == value)
                    return SymbolType.ArgSplit;
            }

            if (Char.IsNumber(value))
                return SymbolType.Numeric;

            if (Char.IsLetter(value))
                return SymbolType.Letter;

            for (int c = 0; c < charAllow.Length; c++)
            {
                if (value == charAllow[c])
                    return SymbolType.Letter;
            }

            return SymbolType.MAX;
        }
    }
}