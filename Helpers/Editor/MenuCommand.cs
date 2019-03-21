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
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Menus
{
    //-------------------------------------------------------------------------
    public static class MenuCommand
    {
        [UnityEditor.MenuItem("CONTEXT/Component/Sort components")]
        public static void SortComponents(UnityEditor.MenuCommand command)
        {
            var behaviour = (Component)command.context;
            var behaviour_object = behaviour.gameObject;

            List<Component> components = new List<Component>();
            List<Component> sorted = new List<Component>();

            Type[] types = new Type[] { typeof(Transform), typeof(MeshFilter), typeof(Renderer), typeof(Collider), typeof(Rigidbody), typeof(MonoBehaviour) };

            behaviour_object.GetComponents(components);
            sorted.AddRange(components);

            sorted.Sort((a, b) =>
            {
                var aType = a.GetType();
                var bType = b.GetType();

                //Sort by type first
                {
                    var length = types.Length;
                    var aIdx = length;
                    var bIdx = length;

                    for (int i = 0; i < length; ++i)
                    {
                        var type = types[i];
                        if (aIdx == length && (aType == type || aType.IsSubclassOf(type)))
                        {
                            aIdx = i;
                        }

                        if (bIdx == length && (bType == type || bType.IsSubclassOf(type)))
                        {
                            bIdx = i;
                        }

                        if (aIdx != length && bIdx != length)
                            break;
                    }

                    if ((aIdx != length || bIdx != length) && aIdx != bIdx)
                    {
                        return aIdx - bIdx;
                    }
                }

                //Types with RequireComponent
                {
                    RequireComponent aRq = null;
                    {
                        var atts = aType.GetCustomAttributes(true);
                        foreach (var att in atts)
                        {
                            aRq = att as RequireComponent;
                            if (aRq != null)
                            {
                                break;
                            }
                        }
                    }

                    RequireComponent bRq = null;
                    {
                        var atts = bType.GetCustomAttributes(true);
                        foreach (var att in atts)
                        {
                            bRq = att as RequireComponent;
                            if (bRq != null)
                            {
                                break;
                            }
                        }
                    }

                    if (aRq == null && bRq != null)
                    {
                        return -1;
                    }
                    else if (aRq != null && bRq == null)
                    {
                        return 1;
                    }
                }

                //Finish ordering by name compare
                return String.Compare(aType.Name, bType.Name);
            });

            for (int iSort = 0; iSort < sorted.Count; ++iSort)
            {
                components.Clear();
                behaviour_object.GetComponents(components);

                var cmp = sorted[iSort];
                int iCmp = components.FindIndex(x => x == cmp);

                int failtest = 20;
                var diff = iSort - iCmp;
                var dir = diff <= 0 ? 1 : -1;
                while (diff != 0)
                {
                    diff += dir;
                    if (dir > 0)
                    {
                        UnityEditorInternal.ComponentUtility.MoveComponentUp(cmp);
                    }
                    else
                    {
                        UnityEditorInternal.ComponentUtility.MoveComponentDown(cmp);
                    }

                    if (--failtest < 0)
                        break;
                }
            }
        }
    }
}
