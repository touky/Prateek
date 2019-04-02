// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.Reflection;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public static class Editors
    {
        //---------------------------------------------------------------------
        public static string[] GetEnumNames(Type type, SerializedProperty property)
        {
            string[] names = null;

            //If the property is an enum, get the names from it
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                names = property.enumNames;
            }

            //Try to get the custom method, if it exist
            if ((names == null || names.Length == 0) && type != null)
            {
                var attribute = type.GetFirstAttribute<EnumMaskMethodAttribute>();
                if (attribute != null)
                {
                    var method = type.GetMethod(attribute.Value);
                    if (method != null)
                    {
                        names = method.Invoke(null, null) as string[];
                    }
                }
            }

            //Not an enum, no custom method, get the type enum names
            if (names == null || names.Length == 0)
            {
                names = Enum.GetNames(type);
            }

            //With custom types, try to find the correct categories/values for these enums
            if (type != null && names != null)
            {
                var values = Enum.GetValues(type);
                if (values.Length == names.Length)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        //Get correct custom names
                        var name = GetEnumAttributeOfType<NameAttribute>((Enum)values.GetValue(i));
                        if (name != null)
                        {
                            names[i] = name.Value;
                        }

                        //Get correct custom categories
                        var category = GetEnumAttributeOfType<CategoryAttribute>((Enum)values.GetValue(i));
                        if (category != null)
                        {
                            if (!category.Value.EndsWith("/"))
                            {
                                names[i] = "/" + names[i];
                            }
                            names[i] = category.Value + names[i];
                        }
                    }
                }
            }

            return names;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="value">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetEnumAttributeOfType<T>(this Enum value) where T : System.Attribute
        {
            var member_info = value.GetType().GetMember(value.ToString());
            var attributes = member_info[0].GetCustomAttributes(typeof(T), false);
            for (int i = 0; i < attributes.Length; i++)
            {
                var typed = attributes[0] as T;
                if (typed != null)
                    return typed;
            }
            return null;
        }

        //---------------------------------------------------------------------
        #region Attributes
        public static T GetFirstAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length > 0)
                return attributes[0] as T;
            return null;
        }

        //---------------------------------------------------------------------
        public static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        //---------------------------------------------------------------------
        public static T GetFirstAttribute<T>(this MemberInfo member_info, bool inherit = false) where T : Attribute
        {
            var attributes = member_info.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length > 0)
                return attributes[0] as T;
            return null;
        }

        //---------------------------------------------------------------------
        public static bool HasAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }
        #endregion Attributes
    }
}
