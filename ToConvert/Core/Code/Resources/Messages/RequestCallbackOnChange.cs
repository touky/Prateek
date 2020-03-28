namespace Mayfair.Core.Code.Resources.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Resources.ResourceTree;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;

    public abstract class RequestCallbackOnChange : RequestMessage, ITreeIdentification
    {
        #region Fields
        private List<string[]> resourceTags = new List<string[]>();
        #endregion

        #region Properties
        public override long MessageID
        {
            get { return ConvertToId(typeof(RequestCallbackOnChange)); }
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