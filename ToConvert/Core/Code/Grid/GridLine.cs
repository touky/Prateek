namespace Mayfair.Core.Code.Grid
{

    using System;
    using UnityEngine;

    [Serializable]
    public struct GridLine
    {
        // Lines just exist on the x axis, relative to the grid object. Start coordinate and length is then sufficient to define a line
        public int x;
        public int y;
        public int length;

        public int End
        {
            get { return this.x + this.length - 1; }
        }

        public GridLine(Vector2Int point)
        {
            this.x = point.x;
            this.y = point.y;
            this.length = 1;
        }
    }
}