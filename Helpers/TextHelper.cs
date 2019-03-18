// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public static partial class StaticText
    {
        //---------------------------------------------------------------------
        public enum Id
        {
            Empty = 0,
        }

        //---------------------------------------------------------------------
        private static Dictionary<Id, string> m_texts = new Dictionary<Id, string>();

        //---------------------------------------------------------------------
        static StaticText()
        { }

        //---------------------------------------------------------------------
        public static void Initialize(EditorData data)
        {
            data.SyncTextInfos();

            m_texts.Clear();
            foreach (var item in data.m_texts)
            {
                m_texts[item.id] = item.text;
            }
        }

        //---------------------------------------------------------------------
        public static string Get(Id id)
        {
            string text = string.Empty;
            Get(id, out text);
            return text;
        }

        public static bool Get(Id id, out string text)
        {
            return m_texts.TryGetValue(id, out text);
        }

        //---------------------------------------------------------------------
        [Serializable]
        public class EditorData
        {
            //---------------------------------------------------------------------------------------------
            [Serializable]
            public class IdSetup
            {
                public Id id;
                public string text;
                public string textDefault;

                public IdSetup(Id newId, string newText)
                {
                    id = newId;
                    text = newText;
                    textDefault = newText;
                }
            }

            static private bool m_textDataHasChanged = false;

            [FormerlySerializedAs("list")]
            public List<IdSetup> m_texts;

            public bool SyncTextInfos()
            {
                m_textDataHasChanged = false;

                var values = Enum.GetValues(typeof(Id));
                if (m_texts.Count != values.Length)
                {
                    foreach (Id value in values)
                    {
                        CheckValue(value, string.Empty);
                    }
                }

                m_texts.Sort((a, b) => (int)a.id - (int)b.id);

                m_textDataHasChanged = 0 != (m_texts.RemoveAll(x =>
                    (int)x.id < 0 || Array.BinarySearch(values, x.id) < 0
                    )) || m_textDataHasChanged;

                //Text IDs --------------------------------------------------------------------------
                CheckValue(Id.Empty, string.Empty);

                return m_textDataHasChanged;
            }

            private void CheckValue(Id id, string text)
            {
                if (m_texts == null)
                    return;

                var index = m_texts.FindIndex(x => x.id == id);
                if (index != -1)
                {
                    if (m_texts[index].id != Id.Empty && m_texts[index].text == String.Empty)
                    {
                        m_texts[index].text = text;
                    }
                    m_texts[index].textDefault = text;
                    return;
                }

                m_texts.Add(new IdSetup(id, text));
                m_textDataHasChanged = true;
            }
        }
    }
}