namespace Prateek.Editor.CodeGeneration.CodeBuilder.ScriptTemplates {
    using System.Collections.Generic;
    using UnityEngine;

    public struct IgnorableContent
    {
        ///-------------------------------------------------------------
        private List<IgnorableBlock> extends;

        ///-------------------------------------------------------------
        public bool IsValid { get { return extends != null && extends.Count > 0; } }

        ///-------------------------------------------------------------
        public void Add(IgnorableBlock ignorableBlock)
        {
            if (extends == null)
                extends = new List<IgnorableBlock>();
            extends.Add(ignorableBlock);
        }

        ///-------------------------------------------------------------
        public int AdvanceToSafety(int index, IgnorableStyle typeToAvoid = IgnorableStyle.MAX)
        {
            if (extends == null)
                return index;

            for (int e = 0; e < extends.Count; e++)
            {
                var extent = extends[e];
                if ((extent.IgnorableStyle & typeToAvoid) != 0 && extent.Contains(index))
                {
                    return extent.OverEnd;
                }
            }

            return index;
        }

        ///-------------------------------------------------------------
        public bool Merge(IgnorableContent other)
        {
            if (other.extends == null)
                return IsValid;

            if (extends == null)
            {
                extends = new List<IgnorableBlock>(other.extends);
                return IsValid;
            }

            var i0     = 0;
            var i1     = 0;
            var result = new List<IgnorableBlock>();
            while (i0 < extends.Count || i1 < other.extends.Count)
            {
                var e = new IgnorableBlock();
                if (i0 >= extends.Count)
                {
                    e = other.extends[i1++];
                }
                else if (i1 >= other.extends.Count)
                {
                    e = extends[i0++];
                }
                else
                {
                    e = extends[i0].Start <= other.extends[i1].Start
                        ? extends[i0++]
                        : other.extends[i1++];
                }

                var hasMerged = false;
                for (int r = 0; r < result.Count; r++)
                {
                    if (result[r].Contains(e.Start) || result[r].Contains(e.End)
                                                    || e.Contains(result[r].Start) || e.Contains(result[r].End))
                    {
                        e = new IgnorableBlock(result[r].IgnorableStyle,
                                               result[r].IsLine,
                                               Mathf.Min(e.Start, result[r].Start),
                                               Mathf.Max(e.End, result[r].End));
                        result[r] = e;
                        hasMerged = true;
                        break;
                    }
                }

                if (!hasMerged)
                {
                    result.Add(e);
                }
            }

            extends = result;
            return IsValid;
        }
    }
}