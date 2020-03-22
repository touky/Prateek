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
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Attributes
{
    using System;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public abstract class EnumBaseAttribute : PropertyAttribute
    { }

    //-------------------------------------------------------------------------
    //Use this on enums to take into account Categories&Names
    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class EnumAllowCategoriesAttribute : EnumBaseAttribute
    {
    }

    //-------------------------------------------------------------------------
    //Use this to treat enum as a mask or to apply an enum mask to an int/ulong/Mask{***}
    //-------------------------------------------------------------------------
    public class EnumMaskAttribute : EnumBaseAttribute
    {
        protected Type value = null;

        public Type Value { get { return value; } }

        public EnumMaskAttribute() { }
        public EnumMaskAttribute(Type enumType)
        {
            value = enumType;
        }
    }

    //-------------------------------------------------------------------------
    //Use this to treat enum as a mask or to apply an enum mask to an int/ulong/Mask{***}
    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EnumMaskMethodAttribute : BaseNameAttribute
    {
        public EnumMaskMethodAttribute(string method) : base(method) { }
    }
}
