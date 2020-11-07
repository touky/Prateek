// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright © 2017-2020 "Touky" <touky@prateek.top>
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

namespace Prateek.Runtime.Core.Helpers
{
    using System.IO;
    using Prateek.Runtime.Core.Consts;

    ///-------------------------------------------------------------------------
    public static class FileInfoExtensions
    {
        ///-------------------------------------------------------------------------
        public static string RelativePath(this FileInfo fileInfo, DirectoryInfo rootInfo)
        {
            if (!fileInfo.FullName.StartsWith(rootInfo.FullName))
            {
                return fileInfo.FullName;
            }

            return fileInfo.FullName.Substring(rootInfo.FullName.Length + Const.NEXT_ITEM);
        }

        ///-------------------------------------------------------------------------
        public static string NameWithoutExtension(this FileInfo fileInfo)
        {
            return Path.GetFileNameWithoutExtension(fileInfo.Name);
        }

        ///-------------------------------------------------------------------------
        ///<summary>
        /// extension is always expected to be lowerCase
        ///</summary>
        public static bool HasExtension(this FileInfo fileInfo, string extension)
        {
            if (fileInfo.Extension == extension)
            {
                return true;
            }

            if (fileInfo.Extension.ToLowerInvariant() == extension)
            {
                return true;
            }

            if (!fileInfo.Extension.StartsWith(Const.DOT_S) || !extension.StartsWith(Const.DOT_S))
            {
                return fileInfo.Extension.TrimStart(Const.DOT_A) == extension.TrimStart(Const.DOT_A);
            }

            return false;
        }
    }
}
