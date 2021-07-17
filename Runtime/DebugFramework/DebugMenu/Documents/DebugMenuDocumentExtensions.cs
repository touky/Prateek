namespace Prateek.Runtime.DebugFramework.DebugMenu.Documents
{
    using Prateek.Runtime.DebugFramework.DebugMenu.Sections;

    public static class DebugMenuDocumentExtensions
    {
        #region Class Methods
        public static TSection Section<TSection>(this DebugMenuDocument document)
            where TSection : DebugMenuSection
        {
            if (document == null)
            {
                return null;
            }

            return document.Get<TSection>();
        }
        #endregion
    }
}
