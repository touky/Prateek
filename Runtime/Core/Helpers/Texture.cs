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
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Extensions;
    using static Extensions.Statics;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public class Textures : SharedStorage
    {
        ///---------------------------------------------------------------------
        public struct Setup
        {
            public Rect inner_rect;
            public Color content;
            public Color border;

            public override string ToString()
            {
                return string.Format("{0:F2}_{1:F2}_{2:F2}_{3:F2}_{4}_{5}",
                                    inner_rect.x, inner_rect.y, inner_rect.width, inner_rect.height,
                                    Format.ToRichText(content),
                                    Format.ToRichText(border));
            }
        }
        private Setup m_setup;

        ///---------------------------------------------------------------------
        private static Textures m_instance = null;
        private static Textures Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Textures();
                return m_instance;
            }
        }

        ///---------------------------------------------------------------------
        public static Texture2D Make(Color content)
        {
            return Make(new Rect(1, 1, 1, 1), content, content);
        }

        ///---------------------------------------------------------------------
        public static Texture2D Make(Color content, Color border)
        {
            return Make(new Rect(1, 1, 1, 1), content, border);
        }

        ///---------------------------------------------------------------------
        public static Texture2D Make(Rect inner_rect, Color content, Color border)
        {
            Instance.m_setup = new Setup()
            {
                inner_rect = inner_rect,
                content = content,
                border = border
            };
            return Instance.GetInstance(Instance.m_setup.ToString()) as Texture2D;
        }

        ///---------------------------------------------------------------------
        protected override object CreateInstance(string key)
        {
            Vector2 size = new Vector2((int)(m_setup.inner_rect.x * 2 + m_setup.inner_rect.width),
                                       (int)(m_setup.inner_rect.y * 2 + m_setup.inner_rect.height));
            Color[] pix = new Color[(int)(size.Area())];

            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = m_setup.inner_rect.Contains(Vectors.FromIndex(i, size)) ? m_setup.content : m_setup.border;
            }

            Texture2D result = new Texture2D((int)size.x, (int)size.y);
            result.name = key;
            result.SetPixels(pix);
            result.Apply();
            result.filterMode = FilterMode.Point;
            return result;
        }

        ///---------------------------------------------------------------------
        public class Drawer
        {
            ///-----------------------------------------------------------------
            public abstract class Base
            {
                ///-------------------------------------------------------------
                public Color color;
                public Vector3 elongate;
                public float fallout;
                public float rounding;
                public float skin;
                private Vector3 position;
                private Quaternion rotation;

                ///-------------------------------------------------------------
                public Base(Vector3 position, Quaternion rotation)
                {
                    this.color = Color.clear;
                    this.fallout = 0;
                    this.position = position;
                    this.rotation = rotation;
                }

                ///-------------------------------------------------------------
                public virtual Color GetColor(Vector3 point)
                {
                    var d = GetDistance(point);
                    if (d < fallout)
                    {
                        if (fallout > 0)
                            return Color.Lerp(color, color.rgbn(0), saturate(d / fallout));
                        return color;
                    }
                    return Color.clear;
                }

                ///-------------------------------------------------------------
                protected Vector3 Elongate(Vector3 point)
                {
                    return point - clamp(point, -elongate, elongate);
                }

                ///-------------------------------------------------------------
                public float GetDistance(Vector3 point)
                {
                    var localPoint = point - position;
                    localPoint = Quaternion.Inverse(rotation) * localPoint;

                    if (!Vectors.Test(elongate, Vector3.zero))
                    {
                        point = Elongate(point);
                    }

                    var d = Distance(localPoint);
                    if (rounding > 0)
                    {
                        d -= rounding;
                    }

                    if (skin > 0)
                    {
                        d = abs(d) - skin;
                    }

                    return d;
                }

                ///-------------------------------------------------------------
                public virtual float Distance(Vector3 point)
                {
                    return float.MaxValue;
                }
            }

            ///-----------------------------------------------------------------
            public class Sphere : Base
            {
                ///-------------------------------------------------------------
                private float size;

                ///-------------------------------------------------------------
                public Sphere(float size) : this(size, Vector3.zero, Quaternion.identity) { }
                public Sphere(float size, Vector3 position) : this(size, position, Quaternion.identity) { }
                public Sphere(float size, Vector3 position, Quaternion rotation) : base(position, rotation)
                {
                    this.size = size;
                }

                ///-------------------------------------------------------------
                public override float Distance(Vector3 point)
                {
                    return length(point) - size;
                }
            }

            ///-----------------------------------------------------------------
            public class Box : Base
            {
                ///-------------------------------------------------------------
                private Vector3 size;

                ///-------------------------------------------------------------
                public Box(Vector3 size) : this(size, Vector3.zero, Quaternion.identity) { }
                public Box(Vector3 size, Vector3 position) : this(size, position, Quaternion.identity) { }
                public Box(Vector3 size, Vector3 position, Quaternion rotation) : base(position, rotation)
                {
                    this.size = size;
                }

                ///-------------------------------------------------------------
                public override float Distance(Vector3 point)
                {
                    var d = abs(point) - size;
                    return length(max(d, 0))
                           + min(max(d.x, max(d.y, d.z)), 0); // remove this line for an only partially signed sdf 
                }
            }

            ///-----------------------------------------------------------------
            public class Torus : Base
            {
                ///-------------------------------------------------------------
                private Vector2 size;

                ///-------------------------------------------------------------
                public Torus(Vector2 size) : this(size, Vector3.zero, Quaternion.identity) { }
                public Torus(Vector2 size, Vector3 position) : this(size, position, Quaternion.identity) { }
                public Torus(Vector2 size, Vector3 position, Quaternion rotation) : base(position, rotation)
                {
                    this.size = size;
                }

                ///-------------------------------------------------------------
                public override float Distance(Vector3 point)
                {
                    var q = vec2(length(point.xz()) - size.x, point.y);
                    return length(q) - size.y;
                }
            }

            ///-----------------------------------------------------------------
            public class Hexagon : Base
            {
                ///-------------------------------------------------------------
                private Vector2 size;

                ///-------------------------------------------------------------
                public Hexagon(Vector2 size) : this(size, Vector3.zero, Quaternion.identity) { }
                public Hexagon(Vector2 size, Vector3 position) : this(size, position, Quaternion.identity) { }
                public Hexagon(Vector2 size, Vector3 position, Quaternion rotation) : base(position, rotation)
                {
                    this.size = size;
                }

                ///-------------------------------------------------------------
                public override float Distance(Vector3 point)
                {
                    var k = vec3(-0.8660254f, 0.5f, 0.57735f);
                    point = point.xzy();
                    point = abs(point);
                    point = vec3(point.xy() - 2.0f * min(dot(k.xy(), point.xy()), 0.0f) * k.xy(), point.z);
                    var d = vec2(
                       length(point.xy() - vec2(clamp(point.x, -k.z * size.x, k.z * size.x), size.x)) * sign(point.y - size.x),
                       point.z - size.y);
                    return min(max(d.x, d.y), 0.0f) + length(max(d, 0.0f));
                }
            }

            ///-----------------------------------------------------------------
            public class Triangle : Base
            {
                ///-------------------------------------------------------------
                private Vector3 a;
                private Vector3 b;
                private Vector3 c;

                ///-------------------------------------------------------------
                public Triangle(Vector3 a, Vector3 b, Vector3 c) : this(a, b, c, Vector3.zero, Quaternion.identity) { }
                public Triangle(Vector3 a, Vector3 b, Vector3 c, Vector3 position) : this(a, b, c, position, Quaternion.identity) { }
                public Triangle(Vector3 a, Vector3 b, Vector3 c, Vector3 position, Quaternion rotation) : base(position, rotation)
                {
                    this.a = a;
                    this.b = b;
                    this.c = c;
                }

                ///-------------------------------------------------------------
                private float dot2(Vector3 v) { return dot(v, v); }
                public override float Distance(Vector3 point)
                {
                    var ba = b - a; var pa = point - a;
                    var cb = c - b; var pb = point - b;
                    var ac = a - c; var pc = point - c;
                    var nor = cross(ba, ac);

                    return sqrt(
                    (sign(dot(cross(ba, nor), pa)) +
                     sign(dot(cross(cb, nor), pb)) +
                     sign(dot(cross(ac, nor), pc)) < 2.0)
                     ?
                     min(min(
                     dot2(ba * clamp(dot(ba, pa) / dot2(ba), 0.0f, 1.0f) - pa),
                     dot2(cb * clamp(dot(cb, pb) / dot2(cb), 0.0f, 1.0f) - pb)),
                     dot2(ac * clamp(dot(ac, pc) / dot2(ac), 0.0f, 1.0f) - pc))
                     :
                     dot(nor, pa) * dot(nor, pa) / dot2(nor)) - 0.001f;
                }
            }

            ///-----------------------------------------------------------------
            public class DualBase : Base
            {
                ///-------------------------------------------------------------
                private Base a;
                private Base b;

                ///-------------------------------------------------------------
                public DualBase(Base a, Base b) : base(Vector3.zero, Quaternion.identity)
                {
                    this.a = a;
                    this.b = b;
                }

                ///-------------------------------------------------------------
                public override float Distance(Vector3 point)
                {
                    float d1 = a.GetDistance(point);
                    float d2 = b.GetDistance(point);
                    return Apply(d1, d2);
                }

                ///-------------------------------------------------------------
                protected virtual float Apply(float d1, float d2)
                {
                    return float.MaxValue;
                }
            }

            ///-----------------------------------------------------------------
            public class Union : DualBase
            {
                public Union(Base a, Base b) : base(a, b) { }
                protected override float Apply(float d1, float d2) { return min(d1, d2); }
            }

            ///-----------------------------------------------------------------
            public class Substraction : DualBase
            {
                public Substraction(Base a, Base b) : base(a, b) { }
                protected override float Apply(float d1, float d2) { return max(d1, -d2); }
            }

            ///-----------------------------------------------------------------
            public class Intersection : DualBase
            {
                public Intersection(Base a, Base b) : base(a, b) { }
                protected override float Apply(float d1, float d2) { return max(d1, d2); }
            }

            ///-----------------------------------------------------------------
            public class SmoothUnion : DualBase
            {
                private float smooth;
                public SmoothUnion(Base a, Base b, float smooth) : base(a, b) { this.smooth = smooth; }
                protected override float Apply(float d1, float d2)
                {
                    float h = clamp(0.5f + 0.5f * (d2 - d1) / smooth, 0.0f, 1.0f);
                    return mix(d2, d1, h) - smooth * h * (1.0f - h);
                }
            }

            ///-----------------------------------------------------------------
            public class SmoothSubstraction : DualBase
            {
                private float smooth;
                public SmoothSubstraction(Base a, Base b, float smooth) : base(a, b) { this.smooth = smooth; }
                protected override float Apply(float d1, float d2)
                {
                    float h = clamp(0.5f - 0.5f * (d2 + d1) / smooth, 0.0f, 1.0f);
                    return mix(d2, -d1, h) + smooth * h * (1.0f - h);
                }
            }

            ///-----------------------------------------------------------------
            public class SmoothIntersection : DualBase
            {
                private float smooth;
                public SmoothIntersection(Base a, Base b, float smooth) : base(a, b) { this.smooth = smooth; }
                protected override float Apply(float d1, float d2)
                {
                    float h = clamp(0.5f - 0.5f * (d2 - d1) / smooth, 0.0f, 1.0f);
                    return mix(d2, d1, h) + smooth * h * (1.0f - h);
                }
            }

            ///-----------------------------------------------------------------
            private Rect rect;
            private Color background;
            private List<Base> operations = new List<Base>();
            private Color[] colors;
            public Texture2D texture;

            ///-----------------------------------------------------------------
            public void Init(Color background, int width, int height, Rect rect, string name)
            {
                this.background = background;
                this.rect = rect;

                if (texture != null)
                {
                    if (texture.width == width && texture.height == height)
                        return;
                    GameObject.Destroy(texture);
                }

                colors = new Color[width * height];
                texture = new Texture2D(width, height);
                texture.name = name;
                texture.filterMode = FilterMode.Point;
                texture.Apply();
            }

            ///-----------------------------------------------------------------
            public void Clear()
            {
                operations.Clear();
            }

            ///-----------------------------------------------------------------
            public void Add(Base operation)
            {
                operations.Add(operation);
            }

            ///-----------------------------------------------------------------
            public void Make()
            {
                
                for (int c = 0; c < colors.Length; c++)
                {
                    var point = rect.position + mul(rect.size, div(Float(vec2i(c % texture.width, c / texture.height)), vec2(texture.width, texture.height)));
                    var result = background;
                    for (int o = 0; o < operations.Count; o++)
                    {
                        var operation = operations[o];
                        var color = operation.GetColor(point.xny());
                        var a = max(result.a, color.a);
                        result = Color.Lerp(result.rgbn(a), color.rgbn(a), color.a);
                    }
                    colors[c] = result;
                }
                texture.SetPixels(colors);
                texture.Apply();
            }
        }
    }
}
