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
namespace Prateek.Helpers
{
    using System;
    using System.IO;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public class ByteBlock
    {
        //---------------------------------------------------------------------
        private Stream source;
        private MemoryStream buffer;
        private BinaryReader reader;
        private BinaryWriter writer;

        //---------------------------------------------------------------------
        public long SourcePosition { get { return source.Position; } }
        public bool AtEnd { get { return buffer.Position >= buffer.Length; } }

        //---------------------------------------------------------------------
        #region Setup
        public ByteBlock(Stream source)
        {
            this.source = source;
            buffer = new MemoryStream();
            reader = new BinaryReader(buffer);
            writer = new BinaryWriter(buffer);
        }

        //---------------------------------------------------------------------
        ~ByteBlock()
        {
            reader.Close();
            buffer.Close();
            buffer.Dispose();
        }

        //---------------------------------------------------------------------
        public bool SourceSeek(long offset, SeekOrigin origin = SeekOrigin.Begin)
        {
            if (source != null)
            {
                return offset == source.Seek(offset, origin);
            }

            return true;
        }

        //---------------------------------------------------------------------
        public bool Load(int size, int offset = 0)
        {
            var data = new byte[size];
            var result = source.Read(data, offset, size);
            if (size != result)
                return false;

            buffer.Seek(0, SeekOrigin.Begin);
            buffer.SetLength(0);
            buffer.Write(data, offset, size);
            buffer.Seek(0, SeekOrigin.Begin);

            return true;
        }

        //---------------------------------------------------------------------
        public void ResetCursor()
        {
            buffer.Seek(0, SeekOrigin.Begin);
        }

        //---------------------------------------------------------------------
        public void Seek(int distance)
        {
            buffer.Seek(distance, SeekOrigin.Current);
        }

        #endregion Setup

        //---------------------------------------------------------------------
        #region Base getters
        public byte Byte() { return reader.ReadByte(); }
        public void Byte(byte value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public byte[] GetBytes(long size = 0)
        {
            var result = new byte[size <= 0 ? buffer.Length - buffer.Position : size];
            for (int f = 0; f < result.Length; f++)
            {
                result[f] = Byte();
            }
            return result;
        }
        public void Bytes(byte[] value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public char Char() { return (Char)reader.ReadByte(); }
        public void Char(Char value) { writer.Write((byte)value); }

        //---------------------------------------------------------------------
        public Int16 Int16() { return reader.ReadInt16(); }
        public void Int16(Int16 value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public UInt16 UInt16() { return reader.ReadUInt16(); }
        public void UInt16(UInt16 value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public Int32 Int32() { return reader.ReadInt32(); }
        public void Int32(Int32 value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public UInt32 UInt32() { return reader.ReadUInt32(); }
        public void UInt32(UInt32 value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public Int64 Int64() { return reader.ReadInt64(); }
        public void Int64(Int64 value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public UInt64 UInt64() { return reader.ReadUInt64(); }
        public void UInt64(UInt64 value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public float Float() { return reader.ReadSingle(); }
        public void Float(float value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public double Double() { return reader.ReadDouble(); }
        public void Double(double value) { writer.Write(value); }

        //---------------------------------------------------------------------
        public void Floats(float[] value) { for (int f = 0; f < value.Length; f++) { Float(value[f]); } }
        public float[] Floats(int size)
        {
            var result = new float[size];
            for (int f = 0; f < size; f++)
            {
                result[f] = Float();
            }
            return result;
        }

        //---------------------------------------------------------------------
        public Color ColorByteRGB() { return new Color((float)Byte(), (float)Byte(), (float)Byte(), 255f) / 255f; }
        public void ColorByteRGB(Color value)
        {
            Byte((byte)(value.r * 255f));
            Byte((byte)(value.g * 255f));
            Byte((byte)(value.b * 255f));
        }

        //---------------------------------------------------------------------
        public Color ColorByteRGBA() { return new Color((float)Byte(), (float)Byte(), (float)Byte(), (float)Byte()) / 255f; }
        public void ColorByteRGBA(Color value)
        {
            Byte((byte)(value.r * 255f));
            Byte((byte)(value.g * 255f));
            Byte((byte)(value.b * 255f));
            Byte((byte)(value.a * 255f));
        }

        //---------------------------------------------------------------------
        public Vector3 Vector3() { return new Vector3(Float(), Float(), Float()); }
        public void Vector3(Vector3 value)
        {
            Float(value.x);
            Float(value.y);
            Float(value.z);
        }

        //---------------------------------------------------------------------
        public Bounds Bounds() { return new Bounds(Vector3(), Vector3()); }
        public void Bounds(Bounds value)
        {
            Vector3(value.min);
            Vector3(value.max);
        }

        //---------------------------------------------------------------------
        public void String(string value) { writer.Write(value); }
        public string String(int size, bool ignoreZeros = false)
        {
            var result = string.Empty;
            var hitZero = false;
            for (int c = 0; c < size; c++)
            {
                char val = Char();
                if (ignoreZeros && (hitZero || val == '\0'))
                {
                    hitZero = true;
                    continue;
                }
                result += val;
            }
            return result;
        }
        #endregion Base getters
    }
}