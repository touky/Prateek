namespace Prateek.Editor.AtomicFileFramework.Detection
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using UnityEngine;

    /// <summary>
    ///     This object will go through the content and detect the separate parts of the yaml file
    /// </summary>
    internal class PartDetector
    {
        #region Fields
        public AtomicFileFormat fileFormat;
        public string[] content;
        public int start = 0;
        public int stop = int.MaxValue;
        public List<DetectedPart> parts;
        #endregion

        #region Class Methods
        public void Detect()
        {
            stop = Mathf.Min(stop, content.Length);
            for (var i = start; i < stop; i++)
            {
                var line  = content[i];
                var match = fileFormat.PartStartRegex.Match(line);
                if (match.Success)
                {
                    AddPart(match, i);
                }
            }
        }

        private void AddPart(Match match, int index)
        {
            parts.Add(new DetectedPart
            {
                fileFormat = fileFormat,
                content = content,
                match = match,
                startLine = index
            });
        }
        #endregion
    }
}
