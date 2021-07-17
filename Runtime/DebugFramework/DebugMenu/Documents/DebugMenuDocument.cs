namespace Prateek.Runtime.DebugFramework.DebugMenu.Documents
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.DebugFramework.DebugMenu.Sections;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class DebugMenuDocument
        : DebugMenuObject
        , GadgetTools.IGadget
    {
        #region Fields
        private DebugMenu.IDocumentOwner Owner { get; set; }
        private bool isDocked = true;
        private List<DebugMenuSection> sections = new List<DebugMenuSection>();
        #endregion

        #region Properties
        public bool IsDocked { get { return isDocked; } internal set { isDocked = value; } }
        #endregion

        #region Constructors
        public DebugMenuDocument() : base()
        {
        }
        #endregion

        #region Class Methods
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

                section.SetOwner(Owner);
                sections.Add(section);
            }
        }

        protected override void OnDraw(DebugMenuContext context)
        {
            if (ImGui.Button(isDocked ? "Undock" : "Dock"))
            {
                isDocked = !isDocked;
            }

            for (int s = 0; s < sections.Count; s++)
            {
                var section = sections[s];
                if (s > 0 && section.Settings.HasFlag(DebugMenuSection.Setting.AddSeparatorBefore))
                {
                    ImGui.Separator();
                }

                if (ImGui.CollapsingHeader(section.Title))
                {
                    using (new ScopeIndent())
                    {
                        section.Draw(context);
                    }
                }

                if (s < sections.Count - 1 && section.Settings.HasFlag(DebugMenuSection.Setting.AddSeparatorAfter))
                {
                    ImGui.Separator();
                }
            }
        }
        #endregion

        #region IGadget Members
        public void Awake()
        {
            Owner.SetupDebugDocument(this, out var title);

            this.title = title;
            DebugMenuRegistry.Register(this);
        }

        public void Kill()
        {
            DebugMenuRegistry.Unregister(this);
        }
        #endregion

        internal TSection Get<TSection>()
            where TSection : DebugMenuSection
        {
            return sections.Find((x) => { return x is TSection; }) as TSection;
        }
    }
}
