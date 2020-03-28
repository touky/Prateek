namespace Mayfair.Core.Code.Grid
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class GridFootprint
    {
        [SerializeField]
        private List<GridLine> lines = new List<GridLine>();

        public List<GridLine> Lines
        {
            get => lines;
            set => lines = value;
        }
        
        public void Build(Vector2Int anchor, List<Vector2Int> points)
        {
            this.lines.Clear();
            foreach (Vector2Int point in points)
            {
                Vector2Int localPoint = point - anchor;
                if (this.lines.Count == 0)
                {
                    this.lines.Add(new GridLine(localPoint));
                }
                else
                {
                    bool merged = false;
                    for (int l = 0; l < this.lines.Count; l++)
                    {
                        GridLine line = this.lines[l];
                        if (localPoint.y != line.y)
                        {
                            continue;
                        }

                        if (localPoint.x >= line.x && localPoint.x < line.x + line.length)
                        {
                            merged = true;
                            break;
                        }

                        if (localPoint.x == line.x - 1 || localPoint.x == line.x + line.length)
                        {
                            if (localPoint.x == line.x - 1)
                            {
                                line.x -= 1;
                            }
                            line.length++;

                            this.lines[l] = line;
                            merged = true;
                            break;
                        }
                    }

                    if (merged)
                    {
                        continue;
                    }

                    this.lines.Add(new GridLine(localPoint));
                }
            }

            MergeAdjacentLines();
        }

        private void MergeAdjacentLines()
        {
            //Merge same adjacent lines
            this.lines.Sort((a, b) =>
            {
                if (a.y != b.y)
                {
                    return a.y - b.y;
                }

                return a.x - b.x;
            });

            for (int l = 0; l < this.lines.Count - 1; l++)
            {
                GridLine aLine = this.lines[l];
                GridLine bLine = this.lines[l + 1];
                if (aLine.y != bLine.y)
                {
                    continue;
                }

                if (aLine.End == bLine.x - 1)
                {
                    aLine.length += bLine.length;
                    this.lines.RemoveAt(l + 1);
                    this.lines[l--] = aLine;
                }
            }
        }
    }
}