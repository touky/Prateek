namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using System.Collections.Generic;
    using ImGuiNET;

    public class DebugMenuDocument : DebugMenuObject
    {
        #region Fields
        private bool isDocked = true;
        private List<DebugMenuSection> sections = new List<DebugMenuSection>();
        #endregion

        #region Properties
        public bool IsDocked { get { return isDocked; } internal set { isDocked = value; } }
        #endregion

        #region Constructors
        public DebugMenuDocument(string title) : base(title) { }

        ~DebugMenuDocument()
        {
            Unregister();
        }
        #endregion

        #region Register/Unregister
        public void Register()
        {
            DebugMenuDaemon.Register(this);
        }
        #endregion

        #region Class Methods
        public void Unregister()
        {
            DebugMenuDaemon.Unregister(this);
        }

        /// <summary>
        ///     Add sections to the Document, does not handle parentage
        /// </summary>
        /// <param name="menuPages"></param>
        public void AddSections(params DebugMenuSection[] menuPages)
        {
            for (var p = 0; p < menuPages.Length; p++)
            {
                var section = menuPages[p];
                if (sections.Contains(section))
                {
                    continue;
                }

                sections.Add(section);
            }
        }

        protected override void OnDraw(DebugMenuContext context)
        {
            if (ImGui.Button(isDocked ? "Undock" : "Dock"))
            {
                isDocked = !isDocked;
            }

            foreach (var section in sections)
            {
                if (ImGui.CollapsingHeader(section.Title))
                {
                    ImGui.Indent();
                    {
                        section.Draw(context);
                    }
                    ImGui.Unindent();
                }
            }
        }
        #endregion
    }
}
