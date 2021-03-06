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
namespace Prateek.Runtime.Core.Helpers
{
    using System;
    using Prateek.Runtime.Core.CachedList;

    ///-------------------------------------------------------------------------
    public struct StringBlurp
    {
        ///---------------------------------------------------------------------
        public struct Tag
        {
            public StaticText.Id id;
            public Tag(StaticText.Id new_id)
            {
                id = new_id;
            }

            ///-----------------------------------------------------------------
            public static implicit operator Tag(StaticText.Id new_id)
            {
                return new Tag(new_id);
            }
        }

        ///---------------------------------------------------------------------
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
        private CachedList10<StaticText.Id> m_ids;
        private CachedList10<string> m_tagsStr;
        private CachedList10<StaticText.Id> m_tagsId;

        ///---------------------------------------------------------------------
        public StringBlurp(bool dummy = false)
        {
            m_text = null;
            m_ids = new CachedList10<StaticText.Id>(StaticText.Id.Empty);
            m_tagsStr = new CachedList10<string>(null);
            m_tagsId = new CachedList10<StaticText.Id>(StaticText.Id.Empty);
        }

        ///---------------------------------------------------------------------
        #region Value ctor/Add
        public StringBlurp(Value v0)
            : this(true)
        {
            Add(v0);
        }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public static implicit operator StringBlurp(Value v) { return new StringBlurp(v); }
#region Overload
        public static implicit operator StringBlurp(StaticText.Id v) { return new StringBlurp(v); }
        public static implicit operator StringBlurp(string v) { return new StringBlurp(v); }
        public static implicit operator StringBlurp(int v) { return new StringBlurp(v); }
        public static implicit operator StringBlurp(Tag v) { return new StringBlurp(v); }
        #endregion //Overload

        ///---------------------------------------------------------------------
        public StringBlurp Add(Value value)
        {
            //todo: switch (value.usage)
            //todo: {
            //todo:     case Value.UsageType.TextId: m_ids.Push(value.id); break;
            //todo:     case Value.UsageType.TagString: m_tagsStr.Push(value.tag_str); break;
            //todo:     case Value.UsageType.TagInt: m_tagsStr.Push(string.Format("{0:D}", value.tag_int)); break;
            //todo:     case Value.UsageType.TagTextId: m_tagsId.Push(value.tag_id.id); break;
            //todo: }
            return this;
        }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public string Build()
        {
            string result = (m_text != null ? m_text : "");
            //todo: for (int i = 0; i < m_ids.Count; ++i)
            //todo: {
            //todo:     result += (i == 0 ? "" : " ") + StaticText.Get(m_ids.Peek(i));
            //todo: }
            //todo: 
            //todo: var array = new String[m_tagsStr.Count + m_tagsId.Count];
            //todo: for (int i = 0; i < array.Length; ++i)
            //todo: {
            //todo:     //Add the string tags
            //todo:     if (i < m_tagsStr.Count)
            //todo:     {
            //todo:         array[i] = m_tagsStr.Peek(i);
            //todo:     }
            //todo:     else if (i - m_tagsStr.Count < m_tagsId.Count)
            //todo:     {
            //todo:         array[i] = StaticText.Get(m_tagsId.Peek(i - m_tagsStr.Count));
            //todo:     }
            //todo: 
            //todo:     //Empty null values
            //todo:     if (array[i] == null)
            //todo:     {
            //todo:         array[i] = "";
            //todo:     }
            //todo: }

            return string.Empty; //todo: array.Length > 0 ? String.Format(result, array) : String.Format(result, "");
        }

        ///---------------------------------------------------------------------
        public bool IsValid()
        {
            return m_ids.Count > 0 || m_tagsStr.Count > 0 || m_tagsId.Count > 0;
        }

        ///---------------------------------------------------------------------
        public bool Has(StaticText.Id id)
        {
            if (id == StaticText.Id.Empty)
            {
                return m_ids.Count > 0;
            }
            else
            {
                return m_ids.Count > 0 && m_ids.Contains(id);
            }
        }

        ///---------------------------------------------------------------------
        public StringBlurp SetPrefixText(string text)
        {
            m_text = text;
            return this;
        }

        ///---------------------------------------------------------------------
        public static StringBlurp operator +(StringBlurp b0, StringBlurp b1)
        {
            //todo: for (int i = 0; i < b1.m_ids.Count; ++i)
            //todo: {
            //todo:     b0.m_ids.Push(b1.m_ids.Peek(i));
            //todo: }
            //todo: 
            //todo: for (int i = 0; i < b1.m_tagsStr.Count; ++i)
            //todo: {
            //todo:     b0.m_tagsStr.Push(b1.m_tagsStr.Peek(i));
            //todo: }
            //todo: 
            //todo: for (int i = 0; i < b1.m_tagsId.Count; ++i)
            //todo: {
            //todo:     b0.m_tagsId.Push(b1.m_tagsId.Peek(i));
            //todo: }

            return b0;
        }

        ///---------------------------------------------------------------------
        public void ClearIds()
        {
            m_ids.Clear();
        }

        ///---------------------------------------------------------------------
        public void ClearTags()
        {
            m_tagsStr.Clear();
            m_tagsId.Clear();
        }

        ///---------------------------------------------------------------------
        public void Clear()
        {
            ClearIds();
            ClearTags();
            m_text = string.Empty;
        }
    }
}