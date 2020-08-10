namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using ImGuiNET;
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
        #endregion
    }
}
