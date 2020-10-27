namespace Prateek.Runtime.AppContentFramework.Unity.Addressables.Debug
{
    using ImGuiNET;
    using Prateek.Runtime.AppContentFramework.Daemons.Debug;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.Reflection;

    public class AddressableRegistrySection
        : RegistryServantSection<AddressableRegistryServant>
    {
        #region Fields
        private DebugField<bool> addressSystemInitialized = "addressSystemInitialized";
        #endregion

        #region Constructors
        public AddressableRegistrySection(AddressableRegistryServant owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
        {
            base.OnDraw(context);

            ImGui.Separator();
            var status = addressSystemInitialized ? "INIT DONE" : "PENDING";
            ImGui.Text($"Addressable status: {status}");
        }
        #endregion
    }
}
