namespace Assets.Prateek.CodeGenerator.Code.CodeBuilder {
    using System;

    [Flags]
    public enum OperationApplied
    {
        RelativeDestination = (1 << 0),

        LoadData = (1 << 1),

        ApplyScriptTemplate = (1 << 2),
        ApplyZonedScript = (1 << 3),
        ApplyKeyword = (1 << 4),
        ApplyFixUp = (1 << 5),

        WriteData = (1 << 6),

        ALL = ~0,

        MAX
    }
}