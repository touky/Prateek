namespace Prateek.Editor.CodeGeneration.CodeBuilder.Utils {
    public struct ArgumentRange
    {
        ///---------------------------------------------------------
        private int min;
        private int max;

        ///---------------------------------------------------------
        public bool NoneNeeded { get { return min <= 0 && max <= 0; } }

        ///---------------------------------------------------------
        public static ArgumentRange AtLeast(int value)
        {
            return new ArgumentRange(value, -1);
        }

        public static ArgumentRange Between(int min, int max)
        {
            return new ArgumentRange(min, max);
        }

        public static implicit operator ArgumentRange(int value)
        {
            return new ArgumentRange(value, value);
        }

        ///---------------------------------------------------------
        private ArgumentRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        ///---------------------------------------------------------
        public bool Check(int count)
        {
            if (NoneNeeded && count > 0)
            {
                return false;
            }

            if (min < 0)
                return true;

            if (count < min)
                return false;

            if (max >= 0)
                return count <= max;
            return true;
        }
    }
}