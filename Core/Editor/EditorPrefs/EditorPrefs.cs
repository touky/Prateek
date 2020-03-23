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

#if UNITY_EDITOR

    //Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

    #endregion Prateek Ifdefs
    // -END_PRATEEK_CSHARP_IFDEF-

//Auto activate some of the prateek defines
namespace Prateek.Core.Editor.EditorPrefs
{
    using System.Collections.Generic;
    using Prateek.Core.Code;
    using Prateek.Core.Code.Extensions;
    using UnityEngine;

    //-----------------------------------------------------------------------------
    //-------------------------------------------------------------------------
    public static  partial class Prefs
    {
        
        //---------------------------------------------------------------------
        #region bool
        public static Bools Get(string name, bool defaultValue)
        {
            return new Bools(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Bools : Prefs.TypedStorage<bool>
        {
            //-----------------------------------------------------------------
            public Bools(string name, bool defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(bool newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetBool(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetBool(name, value);
            }
#endif //UNITY_EDITOR
        }
        #endregion bool
        
        //---------------------------------------------------------------------
        #region int
        public static Ints Get(string name, int defaultValue)
        {
            return new Ints(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Ints : Prefs.TypedStorage<int>
        {
            //-----------------------------------------------------------------
            public Ints(string name, int defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(int newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetInt(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetInt(name, value);
            }
#endif //UNITY_EDITOR
        }
        #endregion int
        
        //---------------------------------------------------------------------
        #region float
        public static Floats Get(string name, float defaultValue)
        {
            return new Floats(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Floats : Prefs.TypedStorage<float>
        {
            //-----------------------------------------------------------------
            public Floats(string name, float defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(float newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetFloat(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetFloat(name, value);
            }
#endif //UNITY_EDITOR
        }
        #endregion float
        
        //---------------------------------------------------------------------
        #region string
        public static Strings Get(string name, string defaultValue)
        {
            return new Strings(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Strings : Prefs.TypedStorage<string>
        {
            //-----------------------------------------------------------------
            public Strings(string name, string defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(string newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetString(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetString(name, value);
            }
#endif //UNITY_EDITOR
        }
        #endregion string
        
    }

    //-----------------------------------------------------------------------------
    //-------------------------------------------------------------------------
    public static  partial class Prefs
    {
        
        //---------------------------------------------------------------------
        #region Vector2Int
        public static Vector2Ints Get(string name, Vector2Int defaultValue)
        {
            return new Vector2Ints(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector2Ints : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints x;
            protected Ints y;
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector2Int Value
            {
                get
                {
                    return new Vector2Int(x.Value, y.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector2Ints(string name, Vector2Int defaultValue) : base(name)
            {
                x = new Ints(name + ".x", defaultValue.x);
                y = new Ints(name + ".y", defaultValue.y);
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector2Int
        
        //---------------------------------------------------------------------
        #region Vector3Int
        public static Vector3Ints Get(string name, Vector3Int defaultValue)
        {
            return new Vector3Ints(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector3Ints : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints x;
            protected Ints y;
            protected Ints z;
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector3Int Value
            {
                get
                {
                    return new Vector3Int(x.Value, y.Value, z.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    z.Value = value.z;
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector3Ints(string name, Vector3Int defaultValue) : base(name)
            {
                x = new Ints(name + ".x", defaultValue.x);
                y = new Ints(name + ".y", defaultValue.y);
                z = new Ints(name + ".z", defaultValue.z);
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector3Int
        
        //---------------------------------------------------------------------
        #region Vector2
        public static Vector2s Get(string name, Vector2 defaultValue)
        {
            return new Vector2s(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector2s : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector2 Value
            {
                get
                {
                    return new Vector2(x.Value, y.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector2s(string name, Vector2 defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector2
        
        //---------------------------------------------------------------------
        #region Vector3
        public static Vector3s Get(string name, Vector3 defaultValue)
        {
            return new Vector3s(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector3s : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
            protected Floats z;
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector3 Value
            {
                get
                {
                    return new Vector3(x.Value, y.Value, z.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    z.Value = value.z;
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector3s(string name, Vector3 defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
                z = new Floats(name + ".z", defaultValue.z);
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector3
        
        //---------------------------------------------------------------------
        #region Vector4
        public static Vector4s Get(string name, Vector4 defaultValue)
        {
            return new Vector4s(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector4s : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
            protected Floats z;
            protected Floats w;
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector4 Value
            {
                get
                {
                    return new Vector4(x.Value, y.Value, z.Value, w.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    z.Value = value.z;
                    w.Value = value.w;
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector4s(string name, Vector4 defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
                z = new Floats(name + ".z", defaultValue.z);
                w = new Floats(name + ".w", defaultValue.w);
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector4
        
        //---------------------------------------------------------------------
        #region Rect
        public static Rects Get(string name, Rect defaultValue)
        {
            return new Rects(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Rects : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
            protected Floats width;
            protected Floats height;
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Rect Value
            {
                get
                {
                    return new Rect(x.Value, y.Value, width.Value, height.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    width.Value = value.width;
                    height.Value = value.height;
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Rects(string name, Rect defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
                width = new Floats(name + ".width", defaultValue.width);
                height = new Floats(name + ".height", defaultValue.height);
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Rect
        
        //---------------------------------------------------------------------
        #region RectInt
        public static RectInts Get(string name, RectInt defaultValue)
        {
            return new RectInts(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class RectInts : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints x;
            protected Ints y;
            protected Ints width;
            protected Ints height;
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public RectInt Value
            {
                get
                {
                    return new RectInt(x.Value, y.Value, width.Value, height.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    width.Value = value.width;
                    height.Value = value.height;
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public RectInts(string name, RectInt defaultValue) : base(name)
            {
                x = new Ints(name + ".x", defaultValue.x);
                y = new Ints(name + ".y", defaultValue.y);
                width = new Ints(name + ".width", defaultValue.width);
                height = new Ints(name + ".height", defaultValue.height);
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion RectInt
        
    }

    //-----------------------------------------------------------------------------
    //-------------------------------------------------------------------------
    public static  partial class Prefs
    {
        
        //---------------------------------------------------------------------
        #region List<string>
        public static ListStrings Get(string name, List<string> default_value)
        {
            return new ListStrings(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListStrings : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Strings> prefValues = new List<Strings>();
            protected List<string> realValues = new List<string>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<string>(new string[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public string this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<string> Values
            {
                get { return new List<string>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListStrings(string name, List<string> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(string));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(string value)
            {
                prefValues.Add(new Strings(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<string> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<string>
        
        //---------------------------------------------------------------------
        #region List<bool>
        public static ListBools Get(string name, List<bool> default_value)
        {
            return new ListBools(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListBools : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Bools> prefValues = new List<Bools>();
            protected List<bool> realValues = new List<bool>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<bool>(new bool[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public bool this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<bool> Values
            {
                get { return new List<bool>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListBools(string name, List<bool> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(bool));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(bool value)
            {
                prefValues.Add(new Bools(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<bool> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<bool>
        
        //---------------------------------------------------------------------
        #region List<int>
        public static ListInts Get(string name, List<int> default_value)
        {
            return new ListInts(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListInts : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Ints> prefValues = new List<Ints>();
            protected List<int> realValues = new List<int>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<int>(new int[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public int this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<int> Values
            {
                get { return new List<int>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListInts(string name, List<int> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(int));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(int value)
            {
                prefValues.Add(new Ints(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<int> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<int>
        
        //---------------------------------------------------------------------
        #region List<float>
        public static ListFloats Get(string name, List<float> default_value)
        {
            return new ListFloats(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListFloats : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Floats> prefValues = new List<Floats>();
            protected List<float> realValues = new List<float>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<float>(new float[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public float this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<float> Values
            {
                get { return new List<float>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListFloats(string name, List<float> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(float));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(float value)
            {
                prefValues.Add(new Floats(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<float> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<float>
        
        //---------------------------------------------------------------------
        #region List<ulong>
        public static ListULongs Get(string name, List<ulong> default_value)
        {
            return new ListULongs(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListULongs : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Prefs.ULongs> prefValues = new List<Prefs.ULongs>();
            protected List<ulong> realValues = new List<ulong>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<ulong>(new ulong[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public ulong this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<ulong> Values
            {
                get { return new List<ulong>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListULongs(string name, List<ulong> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(ulong));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(ulong value)
            {
                prefValues.Add(new Prefs.ULongs(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<ulong> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<ulong>
        
        //---------------------------------------------------------------------
        #region List<Vector2Int>
        public static ListVector2Ints Get(string name, List<Vector2Int> default_value)
        {
            return new ListVector2Ints(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListVector2Ints : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Vector2Ints> prefValues = new List<Vector2Ints>();
            protected List<Vector2Int> realValues = new List<Vector2Int>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<Vector2Int>(new Vector2Int[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public Vector2Int this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<Vector2Int> Values
            {
                get { return new List<Vector2Int>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListVector2Ints(string name, List<Vector2Int> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(Vector2Int));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(Vector2Int value)
            {
                prefValues.Add(new Vector2Ints(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<Vector2Int> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<Vector2Int>
        
        //---------------------------------------------------------------------
        #region List<Vector3Int>
        public static ListVector3Ints Get(string name, List<Vector3Int> default_value)
        {
            return new ListVector3Ints(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListVector3Ints : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Vector3Ints> prefValues = new List<Vector3Ints>();
            protected List<Vector3Int> realValues = new List<Vector3Int>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<Vector3Int>(new Vector3Int[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public Vector3Int this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<Vector3Int> Values
            {
                get { return new List<Vector3Int>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListVector3Ints(string name, List<Vector3Int> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(Vector3Int));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(Vector3Int value)
            {
                prefValues.Add(new Vector3Ints(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<Vector3Int> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<Vector3Int>
        
        //---------------------------------------------------------------------
        #region List<Vector2>
        public static ListVector2s Get(string name, List<Vector2> default_value)
        {
            return new ListVector2s(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListVector2s : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Vector2s> prefValues = new List<Vector2s>();
            protected List<Vector2> realValues = new List<Vector2>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<Vector2>(new Vector2[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public Vector2 this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<Vector2> Values
            {
                get { return new List<Vector2>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListVector2s(string name, List<Vector2> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(Vector2));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(Vector2 value)
            {
                prefValues.Add(new Vector2s(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<Vector2> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<Vector2>
        
        //---------------------------------------------------------------------
        #region List<Vector3>
        public static ListVector3s Get(string name, List<Vector3> default_value)
        {
            return new ListVector3s(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListVector3s : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Vector3s> prefValues = new List<Vector3s>();
            protected List<Vector3> realValues = new List<Vector3>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<Vector3>(new Vector3[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public Vector3 this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<Vector3> Values
            {
                get { return new List<Vector3>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListVector3s(string name, List<Vector3> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(Vector3));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(Vector3 value)
            {
                prefValues.Add(new Vector3s(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<Vector3> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<Vector3>
        
        //---------------------------------------------------------------------
        #region List<Vector4>
        public static ListVector4s Get(string name, List<Vector4> default_value)
        {
            return new ListVector4s(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListVector4s : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Vector4s> prefValues = new List<Vector4s>();
            protected List<Vector4> realValues = new List<Vector4>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<Vector4>(new Vector4[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public Vector4 this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<Vector4> Values
            {
                get { return new List<Vector4>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListVector4s(string name, List<Vector4> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(Vector4));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(Vector4 value)
            {
                prefValues.Add(new Vector4s(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<Vector4> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<Vector4>
        
        //---------------------------------------------------------------------
        #region List<Rect>
        public static ListRects Get(string name, List<Rect> default_value)
        {
            return new ListRects(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListRects : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<Rects> prefValues = new List<Rects>();
            protected List<Rect> realValues = new List<Rect>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<Rect>(new Rect[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public Rect this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<Rect> Values
            {
                get { return new List<Rect>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListRects(string name, List<Rect> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(Rect));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(Rect value)
            {
                prefValues.Add(new Rects(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<Rect> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<Rect>
        
        //---------------------------------------------------------------------
        #region List<RectInt>
        public static ListRectInts Get(string name, List<RectInt> default_value)
        {
            return new ListRectInts(name, default_value);
        }
        
        //---------------------------------------------------------------------
        public class ListRectInts : Prefs.ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Ints count;
            protected List<RectInts> prefValues = new List<RectInts>();
            protected List<RectInt> realValues = new List<RectInt>();
            #endregion Fields
        
            //-----------------------------------------------------------------
            public int Count
            {
                get { return count.Value; }
                set
                {
                    if (count.Value == value)
                        return;
                    Values = new List<RectInt>(new RectInt[value]);
                }
            }
        
            //-----------------------------------------------------------------
            public RectInt this[int index]
            {
                get { return prefValues[index].Value; }
                set
                {
                    realValues[index] = value;
                    prefValues[index].Value = realValues[index];
                    realValues[index] = prefValues[index].Value;
                }
            }
        
            //-----------------------------------------------------------------
            public List<RectInt> Values
            {
                get { return new List<RectInt>(realValues); }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    var length     = CSharp.min(Count, valueCount);
                    for (int l = 0; l < length; l++)
                    {
                        prefValues[l].Value = value[l];
                        realValues[l] = prefValues[l].Value;
                    }
        
                    if (Count > valueCount)
                        RemoveRange(valueCount, Count - valueCount);
        
                    if (Count < valueCount)
                        AddRange(value.GetRange(Count, valueCount - Count));
                }
            }
        
            //-----------------------------------------------------------------
            public ListRectInts(string name, List<RectInt> defaultValue) : base(name)
            {
                var valueCount = defaultValue == null ? 0 : defaultValue.Count;
                this.name = name;
                count = new Ints(base.name + ".Count", valueCount);
                var length = count.Value;
                for (int i = 0; i < length; i++)
                {
                    Add(i < valueCount ? defaultValue[i] : default(RectInt));
                }
            }
        
            //-----------------------------------------------------------------
            private string GetName(int index)
            {
                return string.Format("{0}[{1}]", name, index);
            }
        
            //-----------------------------------------------------------------
            public void Add(RectInt value)
            {
                prefValues.Add(new RectInts(GetName(prefValues.Count), value));
                realValues.Add(prefValues.Last().Value);
                count.Value = realValues.Count;
            }
        
            //-----------------------------------------------------------------
            public void AddRange(List<RectInt> value)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Add(value[i]);
                }
            }
        
            //-----------------------------------------------------------------
            public void RemoveAt(int index)
            {
                realValues.RemoveAt(index);
                count.Value = realValues.Count;
                for (int i = index; i + 1 < prefValues.Count; i++)
                {
                    prefValues[i].Value = prefValues[i + 1].Value;
                }
                prefValues.Last().ClearFromPrefs();
                prefValues.RemoveLast();
            }
        
            //-----------------------------------------------------------------
            public void RemoveLast()
            {
                RemoveAt(count.Value - 1);
            }
        
            //-----------------------------------------------------------------
            public void RemoveRange(int index, int length = 1)
            {
                for (int i = 0; i < length; i++)
                {
                    RemoveAt(index);
                }
            }
        
            //-----------------------------------------------------------------
            public void Clear()
            {
                realValues.Clear();
                count.Value = 0;
                for (int i = 0; i < prefValues.Count; i++)
                {
                    prefValues[i].ClearFromPrefs();
                }
                prefValues.Clear();
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<RectInt>
        
    }
}