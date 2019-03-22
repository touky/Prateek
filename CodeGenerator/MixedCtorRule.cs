// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 20/03/2019
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

#region Unity
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.IO;
using Prateek.IO;
using System.Text.RegularExpressions;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.ScriptTemplating
{
    //-------------------------------------------------------------------------
    [InitializeOnLoad]
    class MixedCTorScriptTemplate : TemplateReplacement
    {
        static MixedCTorScriptTemplate()
        {
            NewMixedCTor(Code.Tag.importExtension).Commit();
        }
    }

    //-------------------------------------------------------------------------
    public partial class TemplateReplacement
    {
        //---------------------------------------------------------------------
        protected static MixedCTorRule NewMixedCTor(string extension)
        {
            return new MixedCTorRule(extension);
        }

        //---------------------------------------------------------------------
        [InitializeOnLoad]
        public partial class MixedCTorRule : CodeRule
        {
            //-----------------------------------------------------------------
            public override string ScopeTag { get { return "MIXED_CTOR"; } }

            //-----------------------------------------------------------------
            public MixedCTorRule(string extension) : base(extension) { }

            //-----------------------------------------------------------------
            #region CodeRule override
            public override Code.Tag.KeyRule GetKeyRule(string keyword, int codeDepth)
            {
                if (keyword == Code.Tag.Macro.OperationClass)
                {
                    return new Code.Tag.KeyRule(keyword, codeDepth == 2) { minArgCount = 2 };
                }
                else
                {
                    return base.GetKeyRule(keyword, codeDepth);
                }
            }

            //-----------------------------------------------------------------
            protected override bool DoTreatData(Code.File.Data activeData, Code.Tag.KeyRule keyRule, List<string> args, string data)
            {
                if (keyRule.key == Code.Tag.Macro.OperationClass)
                {
                    activeData.classInfos.Add(new Code.File.Data.ClassInfo()
                    {
                        name = args[0],
                        variables = args.GetRange(1, args.Count - 1)
                    });
                }
                else
                {
                    return base.DoTreatData(activeData, keyRule, args, data);
                }
                return true;
            }
            #endregion CodeRule override

            //-----------------------------------------------------------------
            #region SwizzleRule internal
            protected override void GatherVariants(List<Variant> variants, Code.File.Data data, Code.File.Data.ClassInfo infoSrc, Code.File.Data.ClassInfo infoDst)
            {
            }
            #endregion SwizzleRule internal
        }
    }
}
