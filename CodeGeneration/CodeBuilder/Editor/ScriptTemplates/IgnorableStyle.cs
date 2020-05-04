namespace Prateek.CodeGeneration.CodeBuilder.Editor.ScriptTemplates {
    using System;

    [Flags]
    public enum IgnorableStyle
    {
        Comment = 1,
        Text = 2,

        MAX = ~0
    }
}