namespace Mayfair.Core.Code.Backend.ServiceProviders.PlayFab
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Requests;
    using UnityEngine.Assertions;

    public class PlayFabRequestManager
    {
        /// <summary>
        ///     PlayFab request limit
        ///     10 updates / 15 seconds max
        ///     0.66 updates / second max
        /// </summary>
        /// <remarks>
        ///     https://developer.playfab.com/en-US/CFEBF/limits/CustomData/UserDataValueUpdatesPer15Seconds
        /// </remarks>
        protected const int MILLISECONDS_BETWEEN_REQUESTS = 800;
        protected readonly Queue<IPlayFabRequest> requestQueue;

        private Task processQueueTask;

        public PlayFabRequestManager()
        {
            requestQueue = new Queue<IPlayFabRequest>();
        }

        public void QueueRequest<TRequest>(TRequest request) where TRequest : class, IPlayFabRequest
        {
            Assert.IsNotNull(request);

            requestQueue.Enqueue(request);

            TryStartProcessQueue();
        }

        private void TryStartProcessQueue()
        {
            if (processQueueTask != null)
            {
                return;
            }

            processQueueTask = ProcessQueue();
        }

        private async Task ProcessQueue()
        {
            while (requestQueue.Count > 0)
            {
                await Task.Delay(MILLISECONDS_BETWEEN_REQUESTS);

                IPlayFabRequest request = requestQueue.Dequeue();
                request.Dispatch();
            }

            processQueueTask = null;
        }
    }
}