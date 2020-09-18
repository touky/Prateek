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
namespace Prateek.Runtime.Core.Attributes
{
    using System;

    ///-------------------------------------------------------------------------
    public abstract class BaseNameAttribute : Attribute
    {
        ///---------------------------------------------------------------------
        protected string value;

        ///---------------------------------------------------------------------
        public string Value { get { return value; } }

        ///---------------------------------------------------------------------
        protected BaseNameAttribute(string new_value)
        {
            value = new_value;
        }
    }

    ///-------------------------------------------------------------------------
    //Custom name that can be set to anything
    ///-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class NameAttribute : BaseNameAttribute
    {
        public NameAttribute(string name) : base(name) { }
    }

    ///-------------------------------------------------------------------------
    //Add a category attribute for any of the targets
    //Can be used to store variables in submenus
    ///-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
    public class CategoryAttribute : BaseNameAttribute
    {
        public CategoryAttribute(string name) : base(name) { }
    }
}
