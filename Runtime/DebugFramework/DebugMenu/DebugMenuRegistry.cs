namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.Core.Singleton;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;

    public sealed class DebugMenuRegistry
        : Registry<DebugMenuRegistry>
    {
        #region Fields
        private bool isOpen = false;
        private bool showDemoWindow = false;
        private DebugMenuContext context;
        private List<DebugMenuDocument> documents = new List<DebugMenuDocument>();
        #endregion

        #region Unity Methods
        private void OnDestroy()
        {
            ImGuiUn.Layout -= DrawDebugMenu;
        }
        #endregion

        #region Register/Unregister
        protected override void OnAwake()
        {
            ImGuiUn.Layout += DrawDebugMenu;
        }

        internal static void Register(DebugMenuDocument menuDocument)
        {
            var instance = Instance;
            if (instance == null)
            {
                return;
            }

            instance.documents.Add(menuDocument);
        }
        
        internal static void Unregister(DebugMenuDocument menuDocument)
        {
            var instance = Instance;
            if (instance == null)
            {
                return;
            }

            instance.documents.Remove(menuDocument);
        }
        #endregion

        #region Class Methods
        private void DrawDebugMenu()
        {
            DrawMainMenu();

            if (showDemoWindow)
            {
                ImGui.ShowDemoWindow(ref showDemoWindow);
            }

            DrawMainWindow();
        }

        private void DrawMainMenu()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open debug window")) { isOpen = true; }

                    if (ImGui.MenuItem("Toggle demo window")) { showDemoWindow = true; }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Edit"))
                {
                    if (ImGui.MenuItem("Undo", "CTRL+Z")) { }

                    if (ImGui.MenuItem("Redo", "CTRL+Y", false, false)) { } // Disabled item

                    ImGui.Separator();
                    if (ImGui.MenuItem("Cut", "CTRL+X")) { }

                    if (ImGui.MenuItem("Copy", "CTRL+C")) { }

                    if (ImGui.MenuItem("Paste", "CTRL+V")) { }

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }
        }

        private void DrawMainWindow()
        {
            if (!isOpen)
            {
                return;
            }

            DrawDockedDocuments();

            DrawUndockedDocuments();
        }

        private void DrawDockedDocuments()
        {
            if (ImGui.Begin($"{nameof(DebugMenuRegistry)} window", ref isOpen, ImGuiWindowFlags.MenuBar))
            {
                DrawMenuBar();

                DrawActiveDocuments();
            }
        }

        private void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("View"))
                {
                    foreach (var document in documents)
                    {
                        if (document.IsOpen)
                        {
                            if (ImGui.BeginMenu(document.Title))
                            {
                                if (ImGui.MenuItem("Dock", !document.IsDocked))
                                {
                                    document.IsDocked = true;
                                }

                                ImGui.EndMenu();
                            }
                        }
                        else
                        {
                            if (ImGui.MenuItem($"Open {document.Title}", !document.IsOpen))
                            {
                                document.IsOpen = true;
                                document.IsDocked = true;
                            }
                        }
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }

        private void DrawActiveDocuments()
        {
            if (ImGui.BeginTabBar("Active documents", ImGuiTabBarFlags.Reorderable | ImGuiTabBarFlags.TabListPopupButton | ImGuiTabBarFlags.FittingPolicyScroll))
            {
                foreach (var document in documents)
                {
                    if (!document.IsOpen || !document.IsDocked)
                    {
                        continue;
                    }

                    var docOpen = document.IsOpen;
                    if (ImGui.BeginTabItem(document.Title, ref docOpen, ImGuiTabItemFlags.None))
                    {
                        document.Draw(context);
                        ImGui.EndTabItem();
                    }

                    document.IsOpen = docOpen;
                }

                ImGui.EndTabBar();
            }

            ImGui.End();
        }

        private void DrawUndockedDocuments()
        {
            foreach (var document in documents)
            {
                if (!document.IsOpen || document.IsDocked)
                {
                    continue;
                }

                var docOpen = document.IsOpen;
                if (ImGui.Begin(document.Title, ref docOpen))
                {
                    document.Draw(context);
                    ImGui.End();
                }

                document.IsOpen = docOpen;
            }
        }
        #endregion
    }
}
