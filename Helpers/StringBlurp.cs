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
    public struct StringBlurp
    {
        //---------------------------------------------------------------------
        public struct Tag
        {
            public StaticText.Id id;
            public Tag(StaticText.Id new_id)
            {
                id = new_id;
            }

            //-----------------------------------------------------------------
            public static implicit operator Tag(StaticText.Id new_id)
            {
                return new Tag(new_id);
            }
        }

        //---------------------------------------------------------------------
        public struct Value
        {
            public enum UsageType
            {
                None,

                TextId,
                TagString,
                TagInt,
                TagTextId,
            };
            public UsageType usage;

            public StaticText.Id id;
            public string tag_str;
            public int tag_int;
            public Tag tag_id;

            private Value(bool value)
            {
                usage = UsageType.None;
                id = StaticText.Id.Empty;
                tag_str = null;
                tag_int = 0;
                tag_id = StaticText.Id.Empty;
            }

            public static implicit operator Value(bool value) { return new Value(true); }
            public static implicit operator Value(StaticText.Id value) { return new Value(true) { usage = UsageType.TextId, id = value }; }
            public static implicit operator Value(string value) { return new Value(true) { usage = UsageType.TagString, tag_str = value }; }
            public static implicit operator Value(int value) { return new Value(true) { usage = UsageType.TagInt, tag_int = value }; }
            public static implicit operator Value(Tag value) { return new Value(true) { usage = UsageType.TagTextId, tag_id = value }; }
        }

        private string m_text;
        private Helpers.CachedArray<StaticText.Id> m_ids;
        private Helpers.CachedArray<string> m_tagsStr;
        private Helpers.CachedArray<StaticText.Id> m_tagsId;

        //---------------------------------------------------------------------
        public StringBlurp(bool dummy = false)
        {
            m_text = null;
            m_ids = new Helpers.CachedArray<StaticText.Id>(StaticText.Id.Empty);
            m_tagsStr = new Helpers.CachedArray<string>(null);
            m_tagsId = new Helpers.CachedArray<StaticText.Id>(StaticText.Id.Empty);
        }

        //---------------------------------------------------------------------
        #region Value ctor/Add
        public StringBlurp(Value v0)
            : this(true)
        {
            Add(v0);
        }

        //---------------------------------------------------------------------
#region Overload
        public StringBlurp(Value v0, Value v1)
            : this(v0) { Add(v1); }
        public StringBlurp(Value v0, Value v1, Value v2)
            : this(v0, v1) { Add(v2); }
        public StringBlurp(Value v0, Value v1, Value v2, Value v3)
            : this(v0, v1, v2) { Add(v3); }
        public StringBlurp(Value v0, Value v1, Value v2, Value v3, Value v4)
            : this(v0, v1, v2, v3) { Add(v4); }
        public StringBlurp(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5)
            : this(v0, v1, v2, v3, v4) { Add(v5); }
        public StringBlurp(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6)
            : this(v0, v1, v2, v3, v4, v5) { Add(v6); }
        public StringBlurp(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6, Value v7)
            : this(v0, v1, v2, v3, v4, v5, v6) { Add(v7); }
        public StringBlurp(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6, Value v7, Value v8)
            : this(v0, v1, v2, v3, v4, v5, v6, v7) { Add(v8); }
        public StringBlurp(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6, Value v7, Value v8, Value v9)
            : this(v0, v1, v2, v3, v4, v5, v6, v7, v8) { Add(v9); }
        #endregion //Overload

        //---------------------------------------------------------------------
        public static implicit operator StringBlurp(Value v) { return new StringBlurp(v); }
#region Overload
        public static implicit operator StringBlurp(StaticText.Id v) { return new StringBlurp(v); }
        public static implicit operator StringBlurp(string v) { return new StringBlurp(v); }
        public static implicit operator StringBlurp(int v) { return new StringBlurp(v); }
        public static implicit operator StringBlurp(Tag v) { return new StringBlurp(v); }
        #endregion //Overload

        //---------------------------------------------------------------------
        public StringBlurp Add(Value value)
        {
            switch (value.usage)
            {
                case Value.UsageType.TextId: m_ids.Push(value.id); break;
                case Value.UsageType.TagString: m_tagsStr.Push(value.tag_str); break;
                case Value.UsageType.TagInt: m_tagsStr.Push(string.Format("{0:D}", value.tag_int)); break;
                case Value.UsageType.TagTextId: m_tagsId.Push(value.tag_id.id); break;
            }
            return this;
        }

        //---------------------------------------------------------------------
#region Overload
        public StringBlurp Add(Value v0, Value v1)
        { Add(v0); Add(v1); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2)
        { Add(v0, v1); Add(v2); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2, Value v3)
        { Add(v0, v1, v2); Add(v3); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2, Value v3, Value v4)
        { Add(v0, v1, v2, v3); Add(v4); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5)
        { Add(v0, v1, v2, v3, v4); Add(v5); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6)
        { Add(v0, v1, v2, v3, v4, v5); Add(v6); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6, Value v7)
        { Add(v0, v1, v2, v3, v4, v5, v6); Add(v7); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6, Value v7, Value v8)
        { Add(v0, v1, v2, v3, v4, v5, v6, v7); Add(v8); return this; }
        public StringBlurp Add(Value v0, Value v1, Value v2, Value v3, Value v4, Value v5, Value v6, Value v7, Value v8, Value v9)
        { Add(v0, v1, v2, v3, v4, v5, v6, v7, v8); Add(v9); return this; }
        #endregion //Overload

        #endregion Value ctor/Add

        //---------------------------------------------------------------------
        public string Build()
        {
            string result = (m_text != null ? m_text : "");
            for (int i = 0; i < m_ids.Count; ++i)
            {
                result += (i == 0 ? "" : " ") + StaticText.Get(m_ids.Peek(i));
            }

            var array = new String[m_tagsStr.Count + m_tagsId.Count];
            for (int i = 0; i < array.Length; ++i)
            {
                //Add the string tags
                if (i < m_tagsStr.Count)
                {
                    array[i] = m_tagsStr.Peek(i);
                }
                else if (i - m_tagsStr.Count < m_tagsId.Count)
                {
                    array[i] = StaticText.Get(m_tagsId.Peek(i - m_tagsStr.Count));
                }

                //Empty null values
                if (array[i] == null)
                {
                    array[i] = "";
                }
            }

            return array.Length > 0 ? String.Format(result, array) : String.Format(result, "");
        }

        //---------------------------------------------------------------------
        public bool IsValid()
        {
            return m_ids.Count > 0 || m_tagsStr.Count > 0 || m_tagsId.Count > 0;
        }

        //---------------------------------------------------------------------
        public bool Has(StaticText.Id id)
        {
            if (id == StaticText.Id.Empty)
            {
                return m_ids.Count > 0;
            }
            else
            {
                return m_ids.Count > 0 && m_ids.Has(x => x == id);
            }
        }

        //---------------------------------------------------------------------
        public StringBlurp SetPrefixText(string text)
        {
            m_text = text;
            return this;
        }

        //---------------------------------------------------------------------
        public static StringBlurp operator +(StringBlurp b0, StringBlurp b1)
        {
            for (int i = 0; i < b1.m_ids.Count; ++i)
            {
                b0.m_ids.Push(b1.m_ids.Peek(i));
            }

            for (int i = 0; i < b1.m_tagsStr.Count; ++i)
            {
                b0.m_tagsStr.Push(b1.m_tagsStr.Peek(i));
            }

            for (int i = 0; i < b1.m_tagsId.Count; ++i)
            {
                b0.m_tagsId.Push(b1.m_tagsId.Peek(i));
            }

            return b0;
        }

        //---------------------------------------------------------------------
        public void ClearIds()
        {
            m_ids.Clear();
        }

        //---------------------------------------------------------------------
        public void ClearTags()
        {
            m_tagsStr.Clear();
            m_tagsId.Clear();
        }

        //---------------------------------------------------------------------
        public void Clear()
        {
            ClearIds();
            ClearTags();
            m_text = string.Empty;
        }
    }
}