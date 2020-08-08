namespace Mayfair.Core.Code.SaveGame.Messages
{
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class LoadDataRequest : RequestCommand
    {
        #region Fields
        private List<SaveDataIdentification> identifications = new List<SaveDataIdentification>();
        #endregion

        #region Properties
        public IReadOnlyList<SaveDataIdentification> Identifications
        {
            get { return this.identifications; }
        }
        #endregion
        
        protected override bool ValidateResponse()
        {
            return holder.Validate<LoadDataResponse>();
        }

        #region Class Methods
        public void Add(SaveDataIdentification identification)
        {
            this.identifications.Add(identification);
        }
        #endregion
    }
}
