namespace Mayfair.Core.Editor.Protocol
{
    using System;
    using Mayfair.Core.Code.Database;
    using Mayfair.Core.Code.Database.Interfaces;

    public static class ProtocolHelper
    {
        public static readonly string[] ASSEMBLY_MATCH = { "Mayfair", "Code" };
        public static readonly string[] KEYWORD_IGNORE = { "Empty", "Mockup" };
        public static readonly Type[] TYPE_IGNORE = { typeof(IDatabaseEntry) };

        public const string PROTOCOL_ROOT_SUFFIX = "EmptyProtocolClass";
        public const string PROTOCOL_DATABASE_SUFFIX = "EmptyDatabaseClass";

        public static string GetRootClassForAssembly(string assemblyName)
        {
            return String.Format("{0}{1}", assemblyName, PROTOCOL_ROOT_SUFFIX);
        }

        public static string GetDatabaseClassForAssembly(string assemblyName)
        {
            return String.Format("{0}{1}", assemblyName, PROTOCOL_DATABASE_SUFFIX);
        }
    }
}