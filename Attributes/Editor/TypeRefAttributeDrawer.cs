//
//  Prateek, a library that is "bien pratique"
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

#if PRATEEK_DEBUG
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Drawers
{
    //-------------------------------------------------------------------------
    [CustomPropertyDrawer(typeof(TypeRefAttribute), true)]
    public class TypeRefAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent content)
        {
            var popup_shown = false;
            var type_ref = attribute as TypeRefAttribute;
            if (type_ref.value != null && property.propertyType == SerializedPropertyType.String)
            {
                var types = new List<Type>(Assembly.GetAssembly(type_ref.value).GetTypes());
                for (int i = 0; i < types.Count; i++)
                {
                    if (!types[i].IsSubclassOf(type_ref.value)
                      || types[i].FullName.IndexOf('`') != -1
                      || types[i].IsAbstract)
                    {
                        types.RemoveAt(i--);
                    }
                }

                if (types.Count > 0)
                {
                    var names = new GUIContent[types.Count + 1];
                    names[0] = new GUIContent("-");
                    var index = 0;
                    for (int i = 0; i < types.Count; i++)
                    {
                        if (property.stringValue == types[i].FullName)
                        {
                            index = i + 1;
                        }

                        names[i + 1] = new GUIContent(types[i].FullName.Replace('.', '/'));
                    }
                    popup_shown = true;
                    index = EditorGUI.Popup(rect, content, index, names, EditorStyles.popup);
                    property.stringValue = index > 0 ? types[index - 1].FullName : string.Empty;
                }
            }

            if (!popup_shown)
            {
                EditorGUI.PropertyField(rect, property, content, true);
            }
        }
    }
}