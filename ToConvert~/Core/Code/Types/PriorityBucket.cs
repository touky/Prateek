namespace Mayfair.Core.Code.Types
{
    using Mayfair.Core.Code.Utils.Debug;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PriorityBucket<T1, T2>
    {
        private Dictionary<T1, int> priorities = new Dictionary<T1, int>();
        private List<Queue<T2>> queues;

        public PriorityBucket(Dictionary<T1, int> priorities)
        {
            this.priorities = new Dictionary<T1, int>(priorities);

            queues = new List<Queue<T2>>(priorities.Count);
            foreach(KeyValuePair<T1, int> priorityItem in priorities)
            {
                queues.Add(new Queue<T2>());
            }
        }

        public bool HasElement()
        {
            for (int i = 0; i < queues.Count; i++)
            {
                if (queues[i].Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public T2 Dequeue()
        {
            for (int i = 0; i < queues.Count; i++)
            {
                if (queues[i].Count > 0)
                {
                    return queues[i].Dequeue();
                }
            }

            DebugTools.LogError("[PriorityBucket] - Cannot dequeue an empty queue");
            return default(T2);
        }

        public void Enqueue(T1 item, T2 instance)
        {
            Debug.Assert(priorities.ContainsKey(item), $"[PriorityBucket] {item} is not a valid key in the priority list");
            int priorityIndex = priorities[item];

            Debug.Assert(priorityIndex < queues.Count, $"[PriorityBucket] {priorityIndex} is out of bounds of the queue list");
            queues[priorityIndex].Enqueue(instance);
        }
    }
}