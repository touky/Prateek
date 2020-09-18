namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using ImGuiNET;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.DebugFramework.Reflection;

    public static class DebugFieldExtensions
    {
        #region Class Methods
        public static bool AssertDrawable(this DebugField field)
        {
            if (field.IsValid)
            {
                return true;
            }

            ImGui.Text($"Debug field {field.Name} is invalid !");

            return false;
        }

        public static bool AssertDrawable(this DebugField field, params DebugField[] fields)
        {
            bool allValid = true;
            for (int f = 0; f < fields.Length + Const.ONE_ITEM; f++)
            {
                var target = f == 0 ? field : fields[f - 1];
                if (!target.IsValid)
                {
                    allValid = false;
                    ImGui.Text($"Debug field {target.Name} is invalid !");
                }
            }

            return allValid;
        }
        #endregion
    }
}
