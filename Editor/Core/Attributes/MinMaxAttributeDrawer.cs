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
namespace Prateek.Editor.Core.Attributes
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using Prateek.Runtime.Core.Attributes;
    using UnityEngine.UIElements;

    ///-------------------------------------------------------------------------
    [CustomPropertyDrawer(typeof(MathAttribute), true)]
    public class MinMaxAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent content)
        {
            var att = attribute as MathAttribute;
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                {
                    property.intValue = Math.Max((int)(att.Type == MathAttribute.ValueType.Int ? att.iMin : att.fMin),
                                        Math.Min((int)(att.Type == MathAttribute.ValueType.Int ? att.iMax : att.fMax),
                                                 property.intValue));
                    break;
                }
                case SerializedPropertyType.Float:
                {
                    property.floatValue = Math.Max((float)(att.Type == MathAttribute.ValueType.Int ? att.iMin : att.fMin),
                                          Math.Min((float)(att.Type == MathAttribute.ValueType.Int ? att.iMax : att.fMax),
                                                   property.floatValue));
                    break;
                }
            }

            EditorGUI.PropertyField(rect, property, content);
        }
    }
}