namespace Prateek.Runtime.Core.Interfaces.IDebuggerDisplay
{
    using System;
    using System.Diagnostics;

    public interface IDebuggerDisplay
    {
        string DebuggerDisplay { get; }
    }

    public static class ConstDebuggerDisplay
    {
        public const string Key = "{DebuggerDisplay,nq}";
    }
}