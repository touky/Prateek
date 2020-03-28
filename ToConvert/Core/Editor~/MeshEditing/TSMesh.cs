namespace Mayfair.Core.Editor.MeshEditing
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils.Extensions;
    using UnityEngine;

    [Serializable]
    public class TSMesh
    {
        #region ExtrudeUVMode enum
        public enum ExtrudeUVMode { Default, UniformU, UniformV }
        #endregion

        #region UVFormat enum
        public enum UVFormat { Error, Vector2, Vector2_1, Vector2_2, Vector3, Vector3_1 }
        #endregion

        #region Fields
        public List<Vector3> vertices = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>();
        public List<Vector4> tangents = new List<Vector4>();
        public List<Vector3> tangents1 = new List<Vector3>();
        public List<Vector3> tangents2 = new List<Vector3>();
        public List<Color> colors = new List<Color>();

        public TSUVData[] uvs =
        {
            new TSUVData(0, UVFormat.Vector2),
            new TSUVData(1, UVFormat.Vector2),
            new TSUVData(2, UVFormat.Vector2),
            new TSUVData(3, UVFormat.Vector2),
            new TSUVData(4, UVFormat.Vector2)
        };

        public List<int> triangles = new List<int>();
        public List<int[]> subMeshes = new List<int[]>();

        public TSBounds bounds = new TSBounds(Vector3.zero, Vector3.zero);

        protected MeshTransformOperation transformPile = new MeshTransformOperation(false);
        #endregion

        #region Properties
        public int vertexCount
        {
            get { return this.vertices.Count; }
        }
        #endregion

        #region Constructors
        public TSMesh() { }

        public TSMesh(Mesh mesh)
        {
            From(mesh);
        }
        #endregion

        #region Class Methods
        public virtual void Clear()
        {
            this.vertices.Clear();
            this.normals.Clear();
            this.tangents.Clear();
            this.tangents1.Clear();
            this.tangents2.Clear();
            this.colors.Clear();
            for (int i = 0; i < this.uvs.Length; i++)
            {
                this.uvs[i].Clear();
            }

            this.triangles.Clear();
            this.subMeshes = new List<int[]>();
            this.bounds = new TSBounds(Vector3.zero, Vector3.zero);
        }

        public virtual void From(Mesh source)
        {
            Clear();

            this.vertices.AddRange(source.vertices);
            if (source.normals != null)
            {
                this.normals.AddRange(source.normals);
            }

            if (source.tangents != null)
            {
                this.tangents.AddRange(source.tangents);
                this.tangents1.AddRange(source.vertices);
                this.tangents2.AddRange(source.vertices);
            }

            if (source.colors != null)
            {
                this.colors.AddRange(source.colors);
            }

            for (int i = 0; i < this.uvs.Length; i++)
            {
                this.uvs[i].RetrieveFrom(source);
            }

            this.triangles.AddRange(source.triangles);
            for (int i = 0; i < source.subMeshCount; i++)
            {
                this.subMeshes.Add(source.GetTriangles(i));
            }

            this.bounds = new TSBounds(source.bounds);
        }

        public virtual void WriteMesh(ref Mesh dest)
        {
            if (this.vertices == null || this.vertices.Count <= 65534)
            {
                dest = dest == null ? new Mesh() : dest;
                dest.Clear();
                dest.SetVertices(this.vertices);
                if (this.normals.Count > 0)
                {
                    dest.SetNormals(this.normals);
                }

                if (this.tangents.Count > 0)
                {
                    dest.SetTangents(this.tangents);
                }

                if (this.colors.Count > 0)
                {
                    dest.SetColors(this.colors);
                }

                for (int i = 0; i < this.uvs.Length; i++)
                {
                    this.uvs[i].ExportTo(dest);
                }

                dest.triangles = this.triangles.ToArray();
                if (this.subMeshes.Count > 0)
                {
                    dest.subMeshCount = this.subMeshes.Count;
                    for (int i = 0; i < this.subMeshes.Count; i++)
                    {
                        dest.SetTriangles(this.subMeshes[i], i);
                    }
                }

                dest.RecalculateBounds();
            }
        }

        public virtual void Merge(TSMesh source)
        {
            int oldVerticesCount = this.vertices.Count;
            int oldTrianglesCount = this.triangles.Count;

            this.vertices.AddRange(source.vertices);
            if (source.normals.Count > 0)
            {
                this.normals.AddRange(source.normals);
            }

            if (source.tangents.Count > 0)
            {
                this.tangents.AddRange(source.tangents);
            }

            if (source.tangents1.Count > 0)
            {
                this.tangents1.AddRange(source.tangents1);
            }

            if (source.tangents2.Count > 0)
            {
                this.tangents2.AddRange(source.tangents2);
            }

            if (source.colors.Count > 0)
            {
                this.colors.AddRange(source.colors);
            }

            for (int i = 0; i < this.uvs.Length; i++)
            {
                this.uvs[i].AddRange(source.uvs[i]);
            }

            this.triangles.AddRange(source.triangles);
            for (int i = oldTrianglesCount; i < this.triangles.Count; i++)
            {
                this.triangles[i] += oldVerticesCount;
            }

            for (int i = 0; i < source.subMeshes.Count; i++)
            {
                if (i >= this.subMeshes.Count)
                {
                    this.subMeshes.Add(source.subMeshes[i]);
                }
                else
                {
                    int[] newTris = new int[this.subMeshes[i].Length + source.subMeshes[i].Length];
                    this.subMeshes[i].CopyTo(newTris, 0);
                    for (int n = 0; n < source.subMeshes[i].Length; n++)
                    {
                        newTris[this.subMeshes[i].Length + n] = source.subMeshes[i][n] + oldVerticesCount;
                    }

                    this.subMeshes[i] = newTris;
                }
            }
        }

        public virtual TSMesh Copy()
        {
            TSMesh result = new TSMesh();
            Copy(result, this);
            return result;
        }

        protected virtual void Copy(TSMesh result, TSMesh source)
        {
            result.Clear();

            result.vertices.AddRange(source.vertices);
            if (source.normals.Count > 0)
            {
                result.normals.AddRange(source.normals);
            }

            if (source.tangents.Count > 0)
            {
                result.tangents.AddRange(source.tangents);
            }

            if (source.tangents1.Count > 0)
            {
                result.tangents1.AddRange(source.tangents1);
            }

            if (source.tangents2.Count > 0)
            {
                result.tangents2.AddRange(source.tangents2);
            }

            if (source.colors.Count > 0)
            {
                result.colors.AddRange(source.colors);
            }

            for (int i = 0; i < this.uvs.Length; i++)
            {
                this.uvs[i].AddRange(source.uvs[i]);
            }

            result.triangles.AddRange(source.triangles);

            result.subMeshes = new List<int[]>();
            result.subMeshes.Capacity = source.subMeshes.Count;
            for (int i = 0; i < source.subMeshes.Count; i++)
            {
                result.subMeshes.Add(new int[source.subMeshes[i].Length]);
                source.subMeshes[i].CopyTo(result.subMeshes[i], 0);
            }

            result.bounds = new TSBounds(source.bounds.center, source.bounds.Size);
        }

        public virtual void CommitTransform()
        {
#if USE_COASTER_PROFILING
        Profiler.BeginSample("CommitTransform()");
#endif

            Matrix4x4 pointMx = this.transformPile.pointMx;
            UVTransformOperation uvMx = this.transformPile.uvMx;
            for (int i = 0; i < this.vertices.Count; i++)
            {
                Vector3 vertex = this.vertices[i];
                Vector3 normal = this.normals.Count > 0 ? this.normals[i] : Vector3.zero;
                Vector4 tangent = this.tangents.Count > 0 ? this.tangents[i] : Vector4.zero;

                vertex = pointMx.MultiplyPoint(vertex);
                if (this.normals.Count > 0)
                {
                    normal = pointMx.MultiplyVector(normal);
                }

                if (this.tangents.Count > 0)
                {
                    tangent = pointMx.MultiplyVector(tangent);
                }

                for (int u = 0; u < this.uvs.Length; u++)
                {
                    if (this.uvs[u].Count > 0)
                    {
                        this.uvs[u].MultiplyPoint(u, uvMx[u, 0], uvMx[u, 1]);
                    }
                }

                this.vertices[i] = vertex;
                if (this.normals.Count > 0)
                {
                    this.normals[i] = normal;
                }

                if (this.tangents.Count > 0)
                {
                    this.tangents[i] = tangent;
                }
            }

            this.transformPile = new MeshTransformOperation(false);

#if USE_COASTER_PROFILING
        Profiler.EndSample();
#endif
        }

        public virtual void UVOperation(int channel, Func<Vector4, int, int, Vector4> operation)
        {
            TSUVData uvs = this.uvs[channel];
            int count = uvs.Count;
            for (int i = 0; i < count; i++)
            {
                Vector4 uv = uvs[i];

                uv = operation.Invoke(uv, i, count);

                uvs[i] = uv;
            }

            this.uvs[channel] = uvs;
        }

        public virtual void SetUV(int channel, Vector2 uv)
        {
            TSUVData uvs = this.uvs[channel];
            int count = this.vertices.Count;

            this.uvs[channel].ConvertTo(UVFormat.Vector2);

            InnerSetUV(channel, uv);

            this.uvs[channel] = uvs;
        }

        public virtual void SetUV(int channel, Vector3 uv)
        {
            TSUVData uvs = this.uvs[channel];
            int count = this.vertices.Count;

            this.uvs[channel].ConvertTo(UVFormat.Vector3);

            InnerSetUV(channel, uv);

            this.uvs[channel] = uvs;
        }

        public virtual void SetUV(int channel, Vector4 uv)
        {
            TSUVData uvs = this.uvs[channel];
            int count = this.vertices.Count;

            this.uvs[channel].ConvertTo(UVFormat.Vector3_1);

            InnerSetUV(channel, uv);

            this.uvs[channel] = uvs;
        }

        protected virtual void InnerSetUV(int channel, Vector4 uv)
        {
            TSUVData uvs = this.uvs[channel];
            int count = this.vertices.Count;

            uvs.Resize(count);
            for (int i = 0; i < count; i++)
            {
                uvs[i] = uv;
            }

            this.uvs[channel] = uvs;
        }

        public virtual void RemoveUV(int channel)
        {
            TSUVData uvs = this.uvs[channel];

            uvs.Clear();

            this.uvs[channel] = uvs;
        }

        public void AddColorChannel()
        {
            AddColorChannel(Color.white);
        }

        public void AddColorChannel(Color defaultColor)
        {
            if (this.colors == null || this.colors.Count != this.vertices.Count)
            {
                this.colors.Resize(this.vertices.Count, defaultColor);
            }
        }

        public void AddNormalChannel()
        {
            AddNormalChannel(Vector3.up);
        }

        public void AddNormalChannel(Vector3 defaultValue)
        {
            if (this.normals == null || this.normals.Count != this.vertices.Count)
            {
                this.normals.Resize(this.normals.Count, defaultValue);
            }
        }

        public virtual void SetColor(Color color)
        {
            AddColorChannel();
            for (int i = 0; i < this.colors.Count; i++)
            {
                this.colors[i] = color;
            }
        }

        public virtual void ApplyColor(Color color)
        {
            AddColorChannel();
            for (int i = 0; i < this.colors.Count; i++)
            {
                this.colors[i] += this.colors[i] * color;
            }
        }

        public virtual void ApplyColor(Color color, Vector3 applyNormal)
        {
            AddColorChannel();
            AddNormalChannel();
            for (int i = 0; i < this.colors.Count; i++)
            {
                this.colors[i] = this.colors[i] * color * Mathf.Max(0, -Vector3.Dot(applyNormal, this.normals[i]));
            }
        }

        public virtual void Transform(Matrix4x4 matrix)
        {
            this.transformPile.pointMx = this.transformPile.pointMx * matrix;
        }

        public virtual void Offset(Vector3 offset)
        {
            this.transformPile.pointMx = this.transformPile.pointMx * Matrix4x4.Translate(offset);
        }

        public virtual void Rotate(Quaternion rotation)
        {
            this.transformPile.pointMx = this.transformPile.pointMx * Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }

        public virtual void Scale(Vector3 scale)
        {
            this.transformPile.pointMx = this.transformPile.pointMx * Matrix4x4.Scale(scale);
        }

        public void OffsetUV(int channel, Vector2 scale0)
        {
            OffsetUV(channel, scale0, Vector2.zero);
        }

        public virtual void OffsetUV(int channel, Vector2 scale0, Vector2 scale1)
        {
            this.transformPile.uvMx[0, 0] = this.transformPile.uvMx[0, 0] * Matrix4x4.Translate(new Vector3(scale0.x, scale0.y, 0));
            this.transformPile.uvMx[0, 1] = this.transformPile.uvMx[0, 1] * Matrix4x4.Translate(new Vector3(scale1.x, scale1.y, 0));
        }

        public void ScaleUV(int channel, Vector2 scale0)
        {
            ScaleUV(channel, scale0, Vector2.one);
        }

        public virtual void ScaleUV(int channel, Vector2 scale0, Vector2 scale1)
        {
            this.transformPile.uvMx[0, 0] = this.transformPile.uvMx[0, 0] * Matrix4x4.Scale(new Vector3(scale0.x, scale0.y, 0));
            this.transformPile.uvMx[0, 1] = this.transformPile.uvMx[0, 1] * Matrix4x4.Scale(new Vector3(scale1.x, scale1.y, 0));
        }

        public void CalculateTangents()
        {
            if (this.vertices.Count == 0 || this.normals.Count == 0)
            {
                this.tangents.Clear();
                return;
            }

#if USE_COASTER_PROFILING
        Profiler.BeginSample("CalculateTangents()");
#endif

            this.tangents.Resize(this.vertices.Count);
            this.tangents1.Resize(this.vertices.Count);
            this.tangents2.Resize(this.vertices.Count);

            for (int i = 0; i < this.subMeshes.Count; i++)
            {
                for (int n = 0; n < this.subMeshes[i].Length; n += 3)
                {
                    int i1 = this.subMeshes[i][n];
                    int i2 = this.subMeshes[i][n + 1];
                    int i3 = this.subMeshes[i][n + 2];
                    Vector3 v1 = this.vertices[i1];
                    Vector3 v2 = this.vertices[i2];
                    Vector3 v3 = this.vertices[i3];
                    Vector4 uv1 = this.uvs[0][i1];
                    Vector4 uv2 = this.uvs[0][i2];
                    Vector4 uv3 = this.uvs[0][i3];
                    float x1 = v2.x - v1.x;
                    float x2 = v3.x - v1.x;
                    float y1 = v2.y - v1.y;
                    float y2 = v3.y - v1.y;
                    float z1 = v2.z - v1.z;
                    float z2 = v3.z - v1.z;
                    float s1 = uv2.x - uv1.x;
                    float s2 = uv3.x - uv1.x;
                    float t1 = uv2.y - uv1.y;
                    float t2 = uv3.y - uv1.y;
                    float div = s1 * t2 - s2 * t1;
                    float r = div == 0f ? 0f : 1f / div;
                    Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                    Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
                    this.tangents1[i1] += sdir;
                    this.tangents1[i2] += sdir;
                    this.tangents1[i3] += sdir;
                    this.tangents2[i1] += tdir;
                    this.tangents2[i2] += tdir;
                    this.tangents2[i3] += tdir;
                }
            }

            for (int i = 0; i < this.vertices.Count; i++)
            {
                Vector3 n = this.normals[i];
                Vector3 t = this.tangents1[i];
                Vector3.OrthoNormalize(ref n, ref t);
                this.tangents[i] = new Vector4(t.x, t.y, t.z,
                                               Vector3.Dot(Vector3.Cross(n, t), this.tangents2[i]) < 0.0f ? -1.0f : 1.0f);
            }

#if USE_COASTER_PROFILING
        Profiler.EndSample();
#endif
        }
        #endregion
    }
}
