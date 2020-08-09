namespace Prateek.Runtime.CommandFramework.Commands.Core
{
    using System.Diagnostics;
    using UnityEngine.Assertions;

    /// <summary>
    ///     A targeted command that expect the recipient to send a response
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class RequestCommand : TargetedCommand, IRequestCommand
    {
        #region Fields
        protected IResponseHolder holder;
        #endregion

        #region Class Methods
        internal static TRequest Create<TRequest, TResponse>()
            where TRequest : RequestCommand, new()
            where TResponse : ResponseCommand, new()
        {
            var request = Create<TRequest>();
            request.holder = new ResponseHolder<TResponse>();
            request.ValidateResponse();
            return request;
        }

        protected abstract bool ValidateResponse();
        #endregion

        #region IRequestCommand Members
        public TResponse GetResponse<TResponse>(bool requestFailed = false)
            where TResponse : ResponseCommand
        {
            Assert.IsTrue(holder.Validate<TResponse>());

            var response = holder.Create();
            response.Init(this, requestFailed);
            return response as TResponse;
        }
        #endregion

        #region Nested type: IResponseHolder
        protected interface IResponseHolder
        {
            #region Class Methods
            bool Validate<T>();
            ResponseCommand Create();
            #endregion
        }
        #endregion

        #region Nested type: ResponseHolder
        private struct ResponseHolder<TResponse> : IResponseHolder
            where TResponse : ResponseCommand, new()
        {
            public ResponseCommand Create()
            {
                return Create<TResponse>();
            }

            public bool Validate<T>()
            {
                return typeof(TResponse).IsSubclassOf(typeof(T));
            }
        }
        #endregion
    }
}
