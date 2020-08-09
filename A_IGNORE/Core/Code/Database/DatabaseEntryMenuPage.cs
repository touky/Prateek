namespace Mayfair.Core.Code.Database
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Database.Interfaces;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Fields;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Utils.Debug.Reflection;

    internal class DatabaseEntryMenuPage : DebugMenuPage<DatabaseDaemonOverseer>
    {
        #region Fields
        private ReflectedField<Dictionary<Type, List<IDatabaseEntry>>> databaseEntries = "databaseEntries";
        #endregion

        #region Constructors
        public DatabaseEntryMenuPage(DatabaseDaemonOverseer owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(DatabaseDaemonOverseer owner, DebugMenuContext context)
        {
            foreach (Type key in databaseEntries.Value.Keys)
            {
                CategoryField category = GetField<CategoryField>();
                category.Draw(context, $"Entry: {key.Name}");
                if (category.ShowContent)
                {
                    using (new ContextIndentScope(context, 1))
                    {
                        List<IDatabaseEntry> entries = null;
                        if (databaseEntries.Value.TryGetValue(key, out entries))
                        {
                            for (int e = 0; e < entries.Count; e++)
                            {
                                IDatabaseEntry entry = entries[e];
                                LabelField label = GetField<LabelField>();
                                label.Draw(context, $"{entry.IdUnique}");
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
