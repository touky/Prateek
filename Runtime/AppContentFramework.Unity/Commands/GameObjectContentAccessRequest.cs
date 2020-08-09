namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using UnityEngine;

    public class GameObjectContentAccessRequest<TChangeMessage>
        : ContentAccessRequest<GameObject>
    {
        #region Class Methods
        protected override bool ValidateResponse()
        {
            return holder.Validate<GameObjectContentAccessResponse>();
        }
        #endregion
    }
}
