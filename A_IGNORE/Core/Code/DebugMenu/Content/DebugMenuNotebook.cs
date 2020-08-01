namespace Mayfair.Core.Code.DebugMenu.Content
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu.Fields;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Utils;

    /// <summary>
    ///     To implement debug menu content, you need to create a DebugMenuNotebook and add pages to it.
    ///     Note that the notebook can be Registered/Unregistered on the fly, allowing for scene-dependent DebugMenu
    /// </summary>
    public class DebugMenuNotebook : DebugMenuDaemon.AbstractDebugNotebook
    {
        #region Fields
        private List<DebugMenuPage> pages = new List<DebugMenuPage>();
        #endregion

        #region Constructors
        public DebugMenuNotebook(string title) : this(GetShortTitle(title), title) { }

        public DebugMenuNotebook(string shortTitle, string title) : base(title)
        {
            this.title = new TitleField(shortTitle, title, true);
        }
        #endregion

        #region Class Methods
        private static string GetShortTitle(string title)
        {
            //todo List<string> matches = new List<string>(10);
            //todo if (RegexHelper.TryFetchingMatches(title, RegexHelper.UpperCase, matches))
            //todo {
            //todo     string result = string.Empty;
            //todo     foreach (string match in matches)
            //todo     {
            //todo         result += match;
            //todo     }
            //todo 
            //todo     return result;
            //todo }

            return string.Empty;
        }

        /// <summary>
        ///     Add pages to the Notebook, FIFO style
        /// </summary>
        /// <param name="menuPages"></param>
        public void AddPages(params DebugMenuPage[] menuPages)
        {
            for (int p = 0; p < menuPages.Length; p++)
            {
                DebugMenuPage page = menuPages[p];
                if (pages.Contains(page))
                {
                    continue;
                }

                pages.Add(page);
            }
        }

        /// <summary>
        ///     Add pages to the notebook, adding the parent first, and parenting the children next
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="menuPages"></param>
        public void AddPagesWithParent(DebugMenuPage parent, params DebugMenuPage[] menuPages)
        {
            AddPages(parent);

            for (int p = 0; p < menuPages.Length; p++)
            {
                DebugMenuPage page = menuPages[p];
                if (pages.Contains(page))
                {
                    continue;
                }

                page.SetParent(parent);
                pages.Add(page);
            }
        }

        public override void Draw(DebugMenuContext context)
        {
            title.Draw(context);

            if (title.ShowContent)
            {
                for (int p = 0; p < pages.Count; p++)
                {
                    DebugMenuPage page = pages[p];
                    if (!page.IsActiveInHierachy)
                    {
                        continue;
                    }

                    using (new ContextIndentScope(context, page.ParentCount))
                    {
                        if (page.HasParent)
                        {
                            page.TitleField.Draw(context);
                        }

                        if (page.ShowContent || !page.HasParent)
                        {
                            using (new ContextIndentScope(context, page.ParentCount + 1))
                            {
                                page.Draw(context);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
