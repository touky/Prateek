// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.CodeGenerator.ScriptTemplates
{
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
#if UNITY_EDITOR
    //todo: fix that
    //todo [InitializeOnLoad]
    class CSharpIgnorableLoader : CodeGenerator.ScriptTemplates.ScriptTemplate
    {
        static CSharpIgnorableLoader()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            ScriptTemplate.NewIgnorableCSharp("cs").Commit();
        }
    }
#endif //UNITY_EDITOR

    //-------------------------------------------------------------------------
    public partial class ScriptTemplate
    {
        //---------------------------------------------------------------------
        protected static IgnorableCSharp NewIgnorableCSharp(string extension)
        {
            return new IgnorableCSharp(extension);
        }

        //---------------------------------------------------------------------
        public class IgnorableCSharp : Ignorable
        {
            //-----------------------------------------------------------------
            private struct MatchData
            {
                //-------------------------------------------------------------
                public Style type;
                public bool isLine;
                public string start;
                public string ignore;
                public string end;

                //-------------------------------------------------------------
                public MatchData(Style type, string start, string end, bool isLine = false) : this(type, start, string.Empty, end, isLine) { }
                public MatchData(Style type, string start, string ignore, string end, bool isLine = false)
                {
                    this.type = type;
                    this.isLine = isLine;
                    this.start = start;
                    this.ignore = ignore;
                    this.end = end;
                }
            }

            //-----------------------------------------------------------------
            private MatchData[] datas = new MatchData[]
            {
                new MatchData(Style.Comment, "//", "\n", true),
                new MatchData(Style.Comment, "/*", "*/"),
                new MatchData(Style.Text, "@\"", "\"\"", "\""),
                new MatchData(Style.Text, "\"", "\\\"", "\"", true)
            };

            //-----------------------------------------------------------------
            public IgnorableCSharp(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            public override void Commit()
            {
                CodeGenerator.ScriptTemplate.Add(this);
            }

            //-----------------------------------------------------------------
            public override BuildResult Build(string content)
            {
                var result = default(BuildResult);
                var position = 0;
                while (position < content.Length)
                {
                    var start = int.MaxValue;
                    var foundD = -1;
                    for (int d = 0; d < datas.Length; d++)
                    {
                        var s0 = content.IndexOf(datas[d].start, position);
                        if (s0 >= 0 && s0 < start)
                        {
                            start = s0;
                            foundD = d;
                        }
                    }

                    if (foundD < 0)
                        break;

                    var data = datas[foundD];
                    position = start + data.start.Length;

                    var end = position;
                    while (true)
                    {
                        var ignore = data.ignore == string.Empty ? -1 : content.IndexOf(data.ignore, position);
                        end = content.IndexOf(data.end, position);

                        if (ignore >= 0 && ignore <= end)
                        {
                            position = Mathf.Max(ignore + data.ignore.Length, end + data.end.Length);
                            continue;
                        }

                        break;
                    }

                    position = end + data.end.Length;
                    result.Add(new Extent(data.type, data.isLine, start, end + (data.end.Length - 1)));
                }
                return result;
            }
        }
    }
}

