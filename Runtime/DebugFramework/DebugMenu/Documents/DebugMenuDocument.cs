namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using UnityEngine;

    public class DebugMenuDocument
        : DebugMenuObject
        , IGadget
    {
        #region Fields
        private IDebugMenuDocumentOwner owner;
        private bool isDocked = true;
        private List<DebugMenuSection> sections = new List<DebugMenuSection>();
        #endregion

        #region Properties
        public bool IsDocked { get { return isDocked; } internal set { isDocked = value; } }
        #endregion

        #region Constructors
        public DebugMenuDocument(IDebugMenuDocumentOwner owner) : base()
        {
            this.owner = owner;
        }
        #endregion

        #region Register/Unregister
        internal void Register(string title)
        {
            this.title = title;
            DebugMenuRegistry.Register(this);
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

                section.SetOwner(owner);
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
                    using (new ScopeIndent())
                    {
                        section.Draw(context);
                    }
                }
            }
        }
        #endregion

        #region IGadget Members
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
