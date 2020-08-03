namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Prateek.A_TODO.Runtime.AppContentFramework.ResourceTree;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    internal static class RegexContent
    {
        internal static readonly Regex FOLDER_SPLIT_REGEX = new Regex("([^\\/]+)(?:\\/)+");
    }

    internal static class RegexHelper
    {
        public static Regex FolderSplit
        {
            get { return RegexContent.FOLDER_SPLIT_REGEX; }
        }
    }

    public abstract class RequestAccessToContent
        : RequestCommand
        , ITreeIdentification
    {
        #region Fields
        private List<string[]> resourceTags = new List<string[]>();
        #endregion

        #region Properties
        public override long CommandID
        {
            get { return ConvertToId(typeof(RequestAccessToContent)); }
        }

        protected abstract Type ResourceType { get; }
        #endregion

        #region Class Methods
        public void Init(string[] resourceTags)
        {
            foreach (string resourceTag in resourceTags)
            {
                MatchCollection collection = RegexHelper.FolderSplit.Matches(resourceTag);
                int i = 0;
                string[] tagArray = new string[collection.Count];
                foreach (Match match in collection)
                {
                    tagArray[i++] = match.Groups[match.Groups.Count - 1].Value;
                }

                this.resourceTags.Add(tagArray);
            }
        }
        #endregion

        #region ITreeIdentification Members
        public List<string[]> TreeTags
        {
            get { return resourceTags; }
        }
        #endregion
    }
}