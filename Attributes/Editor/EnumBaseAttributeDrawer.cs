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
namespace Prateek.Drawers
{
    using Prateek.Attributes;
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    [CustomPropertyDrawer(typeof(EnumBaseAttribute), true)]
    public class EnumBaseAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent content)
        {
            var enumAllow = attribute as EnumAllowCategoriesAttribute;
            var enumMask = attribute as EnumMaskAttribute;

            var type = (enumMask == null || enumMask.Value == null) ? fieldInfo.FieldType : enumMask.Value;

            string[] names = Helpers.Editors.GetEnumNames(type, property);
            if (names != null)
            {
                if (enumAllow != null)
                {
                    var contents = new GUIContent[names.Length];
                    for (int i = 0; i < names.Length; i++)
                    {
                        contents[i] = new GUIContent(names[i], names[i]);
                    }
                    property.enumValueIndex = EditorGUI.Popup(rect, content, property.enumValueIndex, contents, EditorStyles.popup);
                }
                else if (enumMask != null)
                {
                    property.intValue = EditorGUI.MaskField(rect, content, property.intValue, names);
                }
            }
        }
    }
}
