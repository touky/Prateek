// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 05/03/19
//
//  Copyright © 2017—2019 Benjamin "Touky" Huet <huet.benjamin@gmail.com>
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Attributes
{
    //-------------------------------------------------------------------------
    public abstract class BaseNameAttribute : Attribute
    {
        protected string m_value;

        public string value { get { return m_value; } }

        protected BaseNameAttribute(string new_value)
        {
            m_value = new_value;
        }
    }

    //-------------------------------------------------------------------------
    //Custom name that can be set to anything
    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class NameAttribute : BaseNameAttribute
    {
        public NameAttribute(string name) : base(name) { }
    }

    //-------------------------------------------------------------------------
    //Add a category attribute for any of the targets
    //Can be used to store variables in submenus
    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
    public class CategoryAttribute : BaseNameAttribute
    {
        public CategoryAttribute(string name) : base(name) { }
    }
}
