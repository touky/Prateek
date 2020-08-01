namespace Mayfair.Core.Code.Statistics
{
    using UnityEngine;

    public struct Statistic
    {
        //private readonly KeywordHolder tags;
        //private int count;

        //public Statistic(KeywordHolder tags)
        //{
        //    this.tags = tags;
        //    count = 0;
        //}

        //public Statistic(KeywordHolder tags, int count) : this(tags)
        //{
        //    this.count = count;
        //}

        //public KeywordHolder Tags
        //{
        //    get { return tags; }
        //}

        public int Count
        {
            get { return 0; }
        }

        public override int GetHashCode()
        {
            //return tags.GetHashCode();
            return 0;
        }

        public void MergePending(Statistic other)
        {
            //count += other.count;
        }

        public void MergeTotal(Statistic other)
        {
            //count = Mathf.Max(0, count + other.count);
        }
    }
}
