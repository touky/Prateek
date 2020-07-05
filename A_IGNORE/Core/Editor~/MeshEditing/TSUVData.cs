namespace Mayfair.Core.Editor.MeshEditing
{
    using System.Collections;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils.Extensions;
    using UnityEngine;

    public struct TSUVData
    {
        #region Fields
        private int channel;
        private TSMesh.UVFormat format;

        private IList uvs;
        #endregion

        #region Properties
        public TSMesh.UVFormat Format
        {
            get { return this.format; }
        }

        public int Count
        {
            get { return this.uvs.Count; }
        }

        public Vector4 this[int index]
        {
            get { return Get(this.format, this.uvs, index); }
            set { Set(this.format, this.uvs, index, value); }
        }
        #endregion

        #region Constructors
        public TSUVData(int channel, TSMesh.UVFormat format)
        {
            this.channel = channel;
            this.format = format;
            this.uvs = null;
            this.uvs = GetValidList(format);
        }
        #endregion

        #region Class Methods
        private IList GetValidList(TSMesh.UVFormat format)
        {
            Debug.Assert(format != TSMesh.UVFormat.Error);

            switch (format)
            {
                case TSMesh.UVFormat.Vector2: { return new List<Vector2>(); }
                case TSMesh.UVFormat.Vector2_1:
                case TSMesh.UVFormat.Vector3:
                {
                    return new List<Vector3>();
                }
                case TSMesh.UVFormat.Vector2_2:
                case TSMesh.UVFormat.Vector3_1:
                {
                    return new List<Vector4>();
                }
            }

            return null;
        }

        private Vector4 Get(TSMesh.UVFormat format, IList list, int index)
        {
            switch (this.format)
            {
                case TSMesh.UVFormat.Vector2:
                {
                    List<Vector2> tList = list as List<Vector2>;
                    return tList[index];
                }
                case TSMesh.UVFormat.Vector2_1:
                case TSMesh.UVFormat.Vector3:
                {
                    List<Vector3> tList = list as List<Vector3>;
                    return tList[index];
                }
                case TSMesh.UVFormat.Vector2_2:
                case TSMesh.UVFormat.Vector3_1:
                {
                    List<Vector4> tList = list as List<Vector4>;
                    return tList[index];
                }
            }

            return Vector4.zero;
        }

        private void Set(TSMesh.UVFormat format, IList list, int index, Vector4 value)
        {
            switch (this.format)
            {
                case TSMesh.UVFormat.Vector2:
                {
                    List<Vector2> tList = list as List<Vector2>;
                    tList[index] = value;
                    break;
                }
                case TSMesh.UVFormat.Vector2_1:
                case TSMesh.UVFormat.Vector3:
                {
                    List<Vector3> tList = list as List<Vector3>;
                    tList[index] = value;
                    break;
                }
                case TSMesh.UVFormat.Vector2_2:
                case TSMesh.UVFormat.Vector3_1:
                {
                    List<Vector4> tList = list as List<Vector4>;
                    tList[index] = value;
                    break;
                }
            }
        }

        public void ConvertTo(TSMesh.UVFormat newFormat)
        {
            if (this.format == newFormat)
            {
                return;
            }

            IList newUVs = GetValidList(newFormat);

            int count = Count;
            newUVs.Resize(count);

            for (int i = 0; i < count; i++)
            {
                Vector4 value = this[i];
                Set(newFormat, newUVs, i, value);
            }

            this.format = newFormat;
            this.uvs = newUVs;
        }
        #endregion

        #region Helpers
        public void Clear()
        {
            this.uvs.Clear();
        }

        public void Resize(int count)
        {
            this.uvs.Resize(count);
        }

        public void ExportTo(Mesh dest)
        {
            switch (this.format)
            {
                case TSMesh.UVFormat.Vector2:
                {
                    List<Vector2> tList = this.uvs as List<Vector2>;
                    dest.SetUVs(this.channel, tList);
                    break;
                }
                case TSMesh.UVFormat.Vector2_1:
                case TSMesh.UVFormat.Vector3:
                {
                    List<Vector3> tList = this.uvs as List<Vector3>;
                    dest.SetUVs(this.channel, tList);
                    break;
                }
                case TSMesh.UVFormat.Vector2_2:
                case TSMesh.UVFormat.Vector3_1:
                {
                    List<Vector4> tList = this.uvs as List<Vector4>;
                    dest.SetUVs(this.channel, tList);
                    break;
                }
            }
        }

        public void RetrieveFrom(Mesh source)
        {
            switch (this.format)
            {
                case TSMesh.UVFormat.Vector2:
                {
                    List<Vector2> tList = this.uvs as List<Vector2>;
                    source.GetUVs(this.channel, tList);
                    break;
                }
                case TSMesh.UVFormat.Vector2_1:
                case TSMesh.UVFormat.Vector3:
                {
                    List<Vector3> tList = this.uvs as List<Vector3>;
                    source.GetUVs(this.channel, tList);
                    break;
                }
                case TSMesh.UVFormat.Vector2_2:
                case TSMesh.UVFormat.Vector3_1:
                {
                    List<Vector4> tList = this.uvs as List<Vector4>;
                    source.GetUVs(this.channel, tList);
                    break;
                }
            }
        }
        #endregion Helpers

        #region Set
        public void Set(List<Vector2> uvs)
        {
            this.uvs.Clear();
            AddRange(uvs);
        }

        public void Set(List<Vector3> uvs)
        {
            this.uvs.Clear();
            AddRange(uvs);
        }

        public void Set(List<Vector4> uvs)
        {
            this.uvs.Clear();
            AddRange(uvs);
        }
        #endregion Set

        #region AddRange
        public void AddRange(List<Vector2> uvs)
        {
            Debug.Assert(this.format == TSMesh.UVFormat.Vector2);

            AddRange(uvs);
        }

        public void AddRange(List<Vector3> uvs)
        {
            Debug.Assert(this.format == TSMesh.UVFormat.Vector2_1 || this.format == TSMesh.UVFormat.Vector3);

            AddRange(uvs);
        }

        public void AddRange(List<Vector4> uvs)
        {
            Debug.Assert(this.format == TSMesh.UVFormat.Vector2_2 || this.format == TSMesh.UVFormat.Vector3_1);

            AddRange(uvs);
        }

        public void AddRange(TSUVData other)
        {
            Debug.Assert(this.format != other.format);

            AddRange(other.uvs);
        }

        //By default, merge into the larger format
        private void AddRange(IList other)
        {
            switch (this.format)
            {
                case TSMesh.UVFormat.Vector2:
                {
                    (this.uvs as List<Vector2>).AddRange(other as List<Vector2>);
                    break;
                }
                case TSMesh.UVFormat.Vector2_1:
                case TSMesh.UVFormat.Vector3:
                {
                    (this.uvs as List<Vector3>).AddRange(other as List<Vector3>);
                    break;
                }
                case TSMesh.UVFormat.Vector2_2:
                case TSMesh.UVFormat.Vector3_1:
                {
                    (this.uvs as List<Vector4>).AddRange(other as List<Vector4>);
                    break;
                }
            }
        }
        #endregion AddRange

        #region Math operations
        public void MultiplyPoint(int index, Matrix4x4 mx0)
        {
            MultiplyPoint(index, mx0, Matrix4x4.identity);
        }

        public void MultiplyPoint(int index, Matrix4x4 mx0, Matrix4x4 mx1)
        {
            if (index < 0 || index >= Count)
            {
                return;
            }

            switch (this.format)
            {
                case TSMesh.UVFormat.Vector2:
                case TSMesh.UVFormat.Vector2_1:
                case TSMesh.UVFormat.Vector2_2:
                {
                    Vector4 v = this[index];
                    Vector3 v0 = new Vector3(v.x, v.y, 0);
                    Vector3 v1 = new Vector3(v.z, v.w, 0);
                    switch (this.format)
                    {
                        case TSMesh.UVFormat.Vector2:
                        {
                            v0 = mx0.MultiplyPoint(v0);
                            break;
                        }
                        case TSMesh.UVFormat.Vector2_1:
                        case TSMesh.UVFormat.Vector2_2:
                        {
                            v0 = mx0.MultiplyPoint(v0);
                            v1 = mx1.MultiplyPoint(v1);
                            break;
                        }
                    }

                    this[index] = new Vector4(v0.x, v0.y, v1.x, v1.y);
                    break;
                }
                case TSMesh.UVFormat.Vector3:
                case TSMesh.UVFormat.Vector3_1:
                {
                    Vector4 v = this[index];
                    Vector3 v0 = new Vector3(v.x, v.y, v.z);
                    Vector3 v1 = new Vector3(v.w, 0, 0);

                    v0 = mx0.MultiplyPoint(v0);
                    v1 = mx1.MultiplyPoint(v1);

                    this[index] = new Vector4(v0.x, v0.y, v0.z, v1.x);
                    break;
                }
            }
        }
        #endregion Math operations
    }
}
