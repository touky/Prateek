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
using System.Reflection;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Drawers
{
    using System;
    using System.Collections.Generic;
    using Prateek.Attributes;
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    [CustomPropertyDrawer(typeof(TypeRefAttribute), true)]
    public class TypeRefAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent content)
        {
            var popupShown = false;
            var typeRef = attribute as TypeRefAttribute;
            if (typeRef.Value != null && property.propertyType == SerializedPropertyType.String)
            {
                var types = new List<Type>(Assembly.GetAssembly(typeRef.Value).GetTypes());
                for (int i = 0; i < types.Count; i++)
                {
                    if (!types[i].IsSubclassOf(typeRef.Value)
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
                    popupShown = true;
                    index = EditorGUI.Popup(rect, content, index, names, EditorStyles.popup);
                    property.stringValue = index > 0 ? types[index - 1].FullName : string.Empty;
                }
            }

            if (!popupShown)
            {
                EditorGUI.PropertyField(rect, property, content, true);
            }
        }
    }
}