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
#if PRATEEK_DEBUG
namespace Prateek.Debug.Code
{
    using System.Collections.Generic;
    using Prateek.Base.Registry;
    using Prateek.CodeGenerator.PrateekScript.ScriptExport;
    using UnityEditor;
    using UnityEngine;

    public class DebugLineDisplayer : MonoBehaviour
    {
        //---------------------------------------------------------------------
        #region Declarations
        public struct LineData
        {
            //-----------------------------------------------------------------
            private const int offset = 20;
            private const string lineKeyword = "LINE_USE_BORDER";

            //-----------------------------------------------------------------
            public struct MeshContainer
            {
                //-------------------------------------------------------------
                public class Bound { public Bounds b; }
                private Bound bounds;
                private MeshRenderer renderer;
                private MeshFilter filter;
                private Mesh mesh;

                private int borderThickness;
                private int[] triangles;
                private Vector3[] vertices;

                //-------------------------------------------------------------
                public Bound Bounds { get { return bounds; } }

                //-------------------------------------------------------------
                public MeshContainer(int capacity, MeshRenderer renderer, int borderThickness)
                {
                    this.bounds = new Bound() { b = new Bounds() };
                    this.renderer = renderer;
                    this.filter = renderer.GetComponent<MeshFilter>();
                    this.mesh = new Mesh();
                    this.mesh.name = "DebugLineMesh";
                    this.filter.sharedMesh = this.mesh;
                    this.borderThickness = borderThickness + 1;
                    
                    vertices = null;
                    triangles = null;
                    RefreshMesh(capacity, borderThickness);
                }

                //-------------------------------------------------------------
                public void Destroy()
                {
                    if (renderer == null)
                        return;

                    filter.sharedMesh = null;

                    renderer = null;
                    filter = null;
                    GameObject.Destroy(mesh);
                    mesh = null;
                    vertices = null;
                }

                //-------------------------------------------------------------
                public void RefreshMesh(int size, int borderThickness)
                {
                    //borderThickness = 0;
                    if (this.borderThickness != borderThickness)
                    {
                        vertices = null;
                        triangles = null;
                        this.borderThickness = borderThickness;
                    }

                    size = CSharp.min(size, (1 << 16) / (borderThickness == 0 ? 4 : 12));

                    var vtxCount = borderThickness == 0 ? 2 : 6;
                    var triCount = borderThickness == 0 ? 3 : 9;
                    var newSize = size * vtxCount * 2;

                    if (vertices == null || newSize > vertices.Length)
                    {
                        var oldVert = vertices;
                        vertices = new Vector3[(size + offset) * vtxCount * 2];
                        if (oldVert != null)
                        {
                            oldVert.CopyTo(vertices, 0);
                        }

                        for (int v = (oldVert == null ? 0 : oldVert.Length); v < vertices.Length; v += vtxCount * 2)
                        {
                            vertices[v + 0] = CSharp.vec3((v / vtxCount) + 0, -1, (v / vtxCount) + 1);
                            vertices[v + 1] = CSharp.vec3((v / vtxCount) + 0, +1, (v / vtxCount) + 1);
                            vertices[v + 2] = CSharp.vec3((v / vtxCount) + 1, +1, (v / vtxCount) + 0);
                            vertices[v + 3] = CSharp.vec3((v / vtxCount) + 1, -1, (v / vtxCount) + 0);
                            if (borderThickness > 0)
                            {
                                vertices[v + 0 + 4] = CSharp.vec3((v / vtxCount) + 0, -4, (v / vtxCount) + 1);
                                vertices[v + 1 + 4] = CSharp.vec3((v / vtxCount) + 0, -3, (v / vtxCount) + 1);
                                vertices[v + 2 + 4] = CSharp.vec3((v / vtxCount) + 1, +4, (v / vtxCount) + 0);
                                vertices[v + 3 + 4] = CSharp.vec3((v / vtxCount) + 1, +3, (v / vtxCount) + 0);

                                vertices[v + 0 + 8] = CSharp.vec3((v / vtxCount) + 0, +3, (v / vtxCount) + 1);
                                vertices[v + 1 + 8] = CSharp.vec3((v / vtxCount) + 0, +4, (v / vtxCount) + 1);
                                vertices[v + 2 + 8] = CSharp.vec3((v / vtxCount) + 1, -3, (v / vtxCount) + 0);
                                vertices[v + 3 + 8] = CSharp.vec3((v / vtxCount) + 1, -4, (v / vtxCount) + 0);
                            }
                        }

                        var oldTris = triangles;
                        triangles = new int[(size + offset) * triCount * 2];
                        if (oldTris != null)
                        {
                            oldTris.CopyTo(triangles, 0);
                        }

                        for (int t = (oldTris == null ? 0 : oldTris.Length); t < triangles.Length; t += triCount * 2)
                        {
                            triangles[t + 0] = (t / triCount) * vtxCount + 0;
                            triangles[t + 1] = (t / triCount) * vtxCount + 1;
                            triangles[t + 2] = (t / triCount) * vtxCount + 2;
                            triangles[t + 3] = (t / triCount) * vtxCount + 1;
                            triangles[t + 4] = (t / triCount) * vtxCount + 3;
                            triangles[t + 5] = (t / triCount) * vtxCount + 2;
                            if (borderThickness > 0)
                            {
                                triangles[t + 0 + 6] = (t / triCount) * vtxCount + 0 + 4;
                                triangles[t + 1 + 6] = (t / triCount) * vtxCount + 1 + 4;
                                triangles[t + 2 + 6] = (t / triCount) * vtxCount + 2 + 4;
                                triangles[t + 3 + 6] = (t / triCount) * vtxCount + 1 + 4;
                                triangles[t + 4 + 6] = (t / triCount) * vtxCount + 3 + 4;
                                triangles[t + 5 + 6] = (t / triCount) * vtxCount + 2 + 4;

                                triangles[t + 0 + 12] = (t / triCount) * vtxCount + 0 + 8;
                                triangles[t + 1 + 12] = (t / triCount) * vtxCount + 1 + 8;
                                triangles[t + 2 + 12] = (t / triCount) * vtxCount + 2 + 8;
                                triangles[t + 3 + 12] = (t / triCount) * vtxCount + 1 + 8;
                                triangles[t + 4 + 12] = (t / triCount) * vtxCount + 3 + 8;
                                triangles[t + 5 + 12] = (t / triCount) * vtxCount + 2 + 8;
                            }
                        }

                        mesh.Clear();
                        mesh.vertices = vertices;
                        mesh.triangles = triangles;
                    }

                    mesh.bounds = bounds.b;
                }
            }

            //-----------------------------------------------------------------
            public struct BufferContainer
            {
                //-------------------------------------------------------------
                private ComputeBuffer buffer;
                private List<Vector4> list;
                private Vector4[] array;

                //-------------------------------------------------------------
                public ComputeBuffer Buffer { get { return buffer; } }
                public int Count { get { return list.Count; } }
                public Vector4 this[int index] { set { list[index] = value; } }

                //-------------------------------------------------------------
                public BufferContainer(int capacity)
                {
                    buffer = new ComputeBuffer(capacity, 16);
                    list = new List<Vector4>(capacity);
                    array = new Vector4[capacity];
                }

                //-------------------------------------------------------------
                public void Destroy()
                {
                    if (list == null)
                        return;

                    buffer.Release();
                    list = null;
                    array = null;
                }

                //-------------------------------------------------------------
                public void Increment(int index)
                {
                    if (index * 2 < list.Count)
                        return;

                    list.Add(Vector4.zero);
                    list.Add(Vector4.zero);
                }

                //-------------------------------------------------------------
                public void Clear()
                {
                    list.Clear();
                }

                //-------------------------------------------------------------
                public void RefreshBuffers()
                {
                    if (list.Count > array.Length)
                    {
                        array = new Vector4[list.Count + offset];
                        if (buffer != null)
                            buffer.Release();
                        buffer = new ComputeBuffer(array.Length + offset, 16);
                    }

                    list.CopyTo(array);
                    buffer.SetData(array);
                }
            }

            //-----------------------------------------------------------------
            private bool forceDirtyPass;
            private int index;
            private GameObject gameObject;
            private Material material;
            private MeshContainer mesh;
            private BufferContainer positions;
            private BufferContainer colors;

            //-----------------------------------------------------------------
            public bool IsDirty { get { return forceDirtyPass || index > 0; } }

            //-----------------------------------------------------------------
            public LineData(int capacity, GameObject gameObject, MeshRenderer renderer, Material material, int borderThickness)
            {
                this.gameObject = gameObject;
                this.material = material;

                forceDirtyPass = false;
                index = 0;
                mesh = new MeshContainer(capacity, renderer, borderThickness);
                positions = new BufferContainer(capacity);
                colors = new BufferContainer(capacity);
            }

            //-----------------------------------------------------------------
            public void Destroy()
            {
                positions.Destroy();
                colors.Destroy();
            }

            //-----------------------------------------------------------------
            public LineSetup GetPoint()
            {
                Increment(index + 1);

                return new LineSetup(mesh.Bounds, index++, positions, colors);
            }

            //-----------------------------------------------------------------
            private void Increment(int index)
            {
                positions.Increment(index);
                colors.Increment(index);
            }

            //-----------------------------------------------------------------
            public void RefreshBuffers(int lineThickness, int borderThickness)
            {
                positions.RefreshBuffers();
                colors.RefreshBuffers();

                mesh.RefreshMesh(positions.Count, borderThickness);

                material.SetBuffer("positionBuffer", positions.Buffer);
                material.SetBuffer("colorBuffer", colors.Buffer);
                material.SetInt("maxVertexShown", positions.Count);
                material.SetFloat("lineThickness", lineThickness);
                material.SetFloat("borderThickness", borderThickness);

                if (borderThickness == 0)
                    material.DisableKeyword(lineKeyword);
                else
                    material.EnableKeyword(lineKeyword);
            }

            //-----------------------------------------------------------------
            public void Clear()
            {
                forceDirtyPass = index != 0;
                index = 0;
                positions.Clear();
                colors.Clear();
            }

            //-----------------------------------------------------------------
            public struct LineSetup
            {
                //-------------------------------------------------------------
                private MeshContainer.Bound bounds;
                private int index;
                private BufferContainer positions;
                private BufferContainer colors;

                //-------------------------------------------------------------
                public LineSetup(MeshContainer.Bound bounds, int index, BufferContainer positions, BufferContainer colors)
                {
                    this.bounds = bounds;
                    this.index = index;
                    this.positions = positions;
                    this.colors = colors;
                }

                //-------------------------------------------------------------
                public void SetLine(Vector3 start, Vector3 end)
                {
                    if (index == 0)
                    {
                        bounds.b = new Bounds(start, Vector3.zero);
                    }

                    bounds.b.Encapsulate(start);
                    bounds.b.Encapsulate(end);

                    positions[index * 2] = start;
                    positions[index * 2 + 1] = end;
                }

                //-------------------------------------------------------------
                public void SetColor(Color front, Color back)
                {
                    colors[index * 2] = front;
                    colors[index * 2 + 1] = back;
                }
            }
        }
        #endregion //Declarations

        //---------------------------------------------------------------------
        #region Fields
        private LineData lineDataBackFront;
        private LineData lineDataFront;
        private int instanceCount = 50;
        #endregion //Fields

        //---------------------------------------------------------------------
        #region Settings
        [SerializeField, Min(1)]
        private int lineThickness = 1;
        [SerializeField, Min(0)]
        private int borderThickness = 1;
        #endregion //Settings

        //---------------------------------------------------------------------
        #region Properties
        public float LineRendererWidth { get { return lineThickness; } }
        #endregion //Properties

        //---------------------------------------------------------------------
        #region Unity Default
        private void OnEnable()
        {
            EnableRendering();
        }

        //---------------------------------------------------------------------
        private void LateUpdate()
        {
            UpdateRendering();
        }

        //---------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnRenderObject()
        {
            if (EditorApplication.isPaused)
                UpdateRendering();
        }
#endif //UNITY_EDITOR

        //---------------------------------------------------------------------
        private void OnDisable()
        {
            DisableRendering();
        }
        #endregion //Unity Default

        //---------------------------------------------------------------------
        #region Lines Pool
        private void EnableRendering()
        {
            for (int p = 0; p < 2; p++)
            {
                var text = p == 0 ? "BackFront" : "Front";
                var lineShader = Shader.Find("Prateek/DebugLineShader" + text);
                var lineMaterial = new Material(lineShader);

                var go = new GameObject("DebugLineRenderer" + text, typeof(MeshFilter), typeof(MeshRenderer));
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;

                var renderer = go.GetComponent<MeshRenderer>();
                renderer.material = lineMaterial;
                lineMaterial = renderer.material;

                if (p == 0)
                    lineDataBackFront = new LineData(instanceCount, go, renderer, lineMaterial, borderThickness);
                else
                    lineDataFront = new LineData(instanceCount, go, renderer, lineMaterial, borderThickness);
            }
        }

        //---------------------------------------------------------------------
        private void UpdateRendering()
        {
            if (!lineDataBackFront.IsDirty || !lineDataBackFront.IsDirty)
                return;

            lineDataBackFront.RefreshBuffers(lineThickness, borderThickness);
            lineDataFront.RefreshBuffers(lineThickness, borderThickness);

            lineDataBackFront.Clear();
            lineDataFront.Clear();
        }

        //---------------------------------------------------------------------
        private void DisableRendering()
        {
            lineDataBackFront.Destroy();
            lineDataFront.Destroy();
        }

        //---------------------------------------------------------------------
        public LineData.LineSetup GetLine(bool useDepth)
        {
            return useDepth ? lineDataBackFront.GetPoint() : lineDataBackFront.GetPoint();
        }
        #endregion //Lines Pool

        //---------------------------------------------------------------------
        public void RenderLine(DebugDraw.DebugStyle setup, Vector3 start, Vector3 end)
        {
            var manager = Registry.GetManager<DebugDisplayManager>();
            if (manager != null)
            {
                if (!manager.IsActive(setup.Flag))
                    return;
            }

            var line = GetLine(setup.DepthTest);
            line.SetLine(start, end);
            line.SetColor(setup.Color, setup.Color);
        }
    }
}
#endif //PRATEEK_DEBUG
