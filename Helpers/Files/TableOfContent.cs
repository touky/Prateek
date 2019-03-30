// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
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
    [Serializable]
    public partial class TableOfContent : ISerializationCallbackReceiver
    {
        //---------------------------------------------------------------------
        #region Declarations
        [Serializable]
        protected struct SerializedTOC
        {
            public Directory directory;
            public int hash;
            public int parentHash;
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        protected Directory root;
        [SerializeField, HideInInspector]
        protected List<SerializedTOC> serializedDatas = new List<SerializedTOC>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public Directory Root { get { return root; } }
        #endregion Properties

        //---------------------------------------------------------------------
        #region Methods
        public TableOfContent()
        {
            root = NewDirectory("Root");
        }

        //---------------------------------------------------------------------
        public virtual Directory NewDirectory(string name)
        {
            return new Directory(name);
        }

        //---------------------------------------------------------------------
        public void Add(File file)
        {
            var fileSplit = file.FullName.Split(Strings.Separator.Directory.Get(), StringSplitOptions.RemoveEmptyEntries);
            var currentDirectory = Root;
            for (int s = 0; s < fileSplit.Length; s++)
            {
                var split = fileSplit[s];
                if (s == fileSplit.Length - 1)
                {
                    currentDirectory.Add(file);
                    break;
                }

                var directoryIndex = currentDirectory.Directories.FindIndex((x) => { return x.Name == split; });
                if (directoryIndex < 0)
                {
                    directoryIndex = currentDirectory.Directories.Count;
                    currentDirectory.Add(NewDirectory(split));
                }
                currentDirectory = currentDirectory.Directories[directoryIndex];
            }
        }

        //---------------------------------------------------------------------
        public override string ToString()
        {
            return ToString(root, "$>");
        }

        //---------------------------------------------------------------------
        private string ToString(Directory directory, string prefix)
        {
            var result = String.Empty;
            var dir = prefix + directory.Name.Directory();
            for (int d = 0; d < directory.Directories.Count; d++)
            {
                var child = directory.Directories[d];
                result += ToString(child, dir);
            }

            result += dir.NewLine();
            for (int f = 0; f < directory.Files.Count; f++)
            {
                var file = directory.Files[f];
                result += file.Name.NewLine();
            }

            return result;
        }
        #endregion Methods

        //---------------------------------------------------------------------
        #region Serialization
        public void OnBeforeSerialize()
        {
            serializedDatas.Clear();
            AddTo(root);
        }

        //---------------------------------------------------------------------
        private void AddTo(Directory directory)
        {
            var data = new SerializedTOC() { directory = directory, hash = directory.GetHashCode(), parentHash = (directory.Parent != null ? directory.Parent.GetHashCode() : 0) };
            serializedDatas.Add(data);
            for (int d = 0; d < directory.Directories.Count; d++)
            {
                AddTo(directory.Directories[d]);
            }
        }

        //---------------------------------------------------------------------
        public void OnAfterDeserialize()
        {
            root = serializedDatas[0].directory;

            for (int d = 1; d < serializedDatas.Count; d++)
            {
                var data = serializedDatas[d];
                if (data.parentHash == 0)
                    continue;

                var parent = serializedDatas.Find((x) => { return x.hash == data.parentHash; });
                data.directory.AfterDeserialize(parent.directory);
            }
        }
        #endregion Serialization

        //---------------------------------------------------------------------
        #region File
        [Serializable]
        public class File
        {
            //-----------------------------------------------------------------
            [Serializable]
            public struct Entry     // A Directory entry
            {
                public int offset;  // Offset to entry, in bytes, from start of file
                public int size;    // Size of entry in file, in bytes

                public static int SizeOf() { return sizeof(int) * 2; }
            }

            //-----------------------------------------------------------------
            [Serializable]
            public struct CustomData
            {
                public enum DataType
                {
                    CustomContent,
                    InnerEntry,

                    MAX
                }

                public DataType type;
                public string name;
                [Space]
                public string content;
                public Entry entry;

                public override string ToString()
                {
                    switch (type)
                    {
                        case DataType.CustomContent: { return String.Format("{0}: {1}", name, content); }
                        case DataType.InnerEntry: { return String.Format("{0}: Offset[{1}], Size[{2}]", name, entry.offset, entry.size); }
                    }
                    return TextMessage.INVALID_FORMAT;
                }
            }

            //-----------------------------------------------------------------
            [SerializeField]
            protected string fullName;
            [SerializeField]
            protected string name;
            [SerializeField]
            protected string extension;

            [SerializeField]
            protected Entry entry = new Entry() { offset = -1, size = -1 };

            [SerializeField, HideInInspector]
            protected List<CustomData> customDatas = new List<CustomData>();

            //-----------------------------------------------------------------
            public string FullName { get { return fullName; } }
            public string Name { get { return name; } }
            public string Extension { get { return extension; } }

            public int Offset { get { return entry.offset; } set { entry.offset = value; } }
            public int Size { get { return entry.size; } set { entry.size = value; } }

            public List<CustomData> CustomDatas { get { return customDatas; } }

            //-----------------------------------------------------------------
            public File(string fullName)
            {
                var fileSplit = fullName.Split(Strings.Separator.Directory.Get(), StringSplitOptions.RemoveEmptyEntries);
                var ExtSplit = fileSplit[fileSplit.Length - 1].Split(Strings.Separator.FileExtension.Get(), StringSplitOptions.RemoveEmptyEntries);

                this.fullName = fullName;

                name = ExtSplit[ExtSplit.Length - 2];
                extension = ExtSplit[ExtSplit.Length - 1];

                entry = new Entry();
            }

            //-----------------------------------------------------------------
            public void AddCustomData(string name, string content)
            {
                customDatas.Add(new CustomData() { name = name, content = content, type = CustomData.DataType.CustomContent });
            }

            //-----------------------------------------------------------------
            public void AddCustomData(string name, Entry entry)
            {
                customDatas.Add(new CustomData() { name = name, entry = entry, type = CustomData.DataType.InnerEntry });
            }
        }
        #endregion File

        //---------------------------------------------------------------------
        #region Directory
        [Serializable]
        public class Directory
        {
            //-----------------------------------------------------------------
            [SerializeField]
            protected string name;
            [SerializeField, HideInInspector]
            protected List<File> files = new List<File>();

            //-----------------------------------------------------------------
            [NonSerialized]
            protected Directory parent;
            [NonSerialized]
            protected List<Directory> directories = new List<Directory>();

            //-----------------------------------------------------------------
            public string Name { get { return name; } }
            public List<File> Files { get { return files; } }

            public Directory Parent { get { return parent; } }
            public List<Directory> Directories { get { return directories; } }

            //-----------------------------------------------------------------
            public Directory(string name)
            {
                this.name = name;
            }

            //-----------------------------------------------------------------
            public void AfterDeserialize(Directory parent)
            {
                if (directories == null)
                {
                    directories = new List<Directory>();
                }

                parent.Add(this);
            }

            //-----------------------------------------------------------------
            public void Add(Directory directory)
            {
                if (directories == null)
                {
                    directories = new List<Directory>();
                }

                directory.parent = this;
                directories.AddUnique(directory);
            }
            public void Add(File file) { files.AddUnique(file); }

            //-----------------------------------------------------------------
            #region GetFiles
            public bool GetFiles(List<File> results, string name, bool fuzzyMatch) { return GetFiles<File>(results, name, fuzzyMatch); }
            public bool GetFiles<F>(List<F> results, string name, bool fuzzyMatch) where F : File
            {
                for (int f = 0; f < files.Count; f++)
                {
                    var file = files[f] as F;
                    if (file != null)
                    {
                        if ((fuzzyMatch && file.FullName.Contains(name))
                        || (!fuzzyMatch && (file.Name == name || file.FullName == name)))
                        {
                            results.AddUnique(file);
                        }
                    }
                }
                return results.Count > 0;
            }

            //-----------------------------------------------------------------
            public bool GetFiles(List<File> results) { return GetFiles<File>(results); }
            public bool GetFiles<F>(List<F> results) where F : File
            {
                for (int f = 0; f < files.Count; f++)
                {
                    var file = files[f] as F;
                    if (file != null)
                        results.AddUnique(file);
                }
                return results.Count > 0;
            }
            #endregion GetFiles

            //-----------------------------------------------------------------
            #region GetFile
            public File GetFile(string name, bool fuzzyMatch) { return GetFile(name, fuzzyMatch); }
            public F GetFile<F>(string name, bool fuzzyMatch) where F : File
            {
                for (int f = 0; f < files.Count; f++)
                {
                    var file = files[f];
                    if (fuzzyMatch && file.FullName.Contains(name))
                        return file as F;
                    else if (file.Name == name || file.FullName == name)
                        return file as F;
                }
                return default(F);
            }

            //-----------------------------------------------------------------
            public File GetFile(int index) { return GetFile(index); }
            public F GetFile<F>(int index) where F : File
            {
                if (index < 0 || index >= files.Count)
                    return default(F);
                return files[index] as F;
            }
            #endregion GetFile
        }
        #endregion Directory
    }
}