namespace Mayfair.Core.Code.SaveGame
{
    using System.Diagnostics;
    using Mayfair.Core.Code.SaveGame.Enums;

    [DebuggerDisplay("{GetType().Name}, File: {FileName}")]
    public abstract class SaveDataIdentification
    {
        #region Fields
        private SaveDataStatusType status = SaveDataStatusType.None;
        private byte[] byteData = null;
        private string stringData = null;
        #endregion

        #region Properties
        public SaveDataStatusType Status
        {
            get { return this.status; }
        }

        public byte[] ByteData
        {
            get { return this.byteData; }
            set { this.byteData = value; }
        }

        public string StringData
        {
            get { return this.stringData; }
            set { this.stringData = value; }
        }

        public abstract string FileName { get; }
        #endregion

        #region Class Methods
        public void CreateEmpty()
        {
            this.status = SaveDataStatusType.CleanState;

            DoCreateEmpty();
        }

        public void LoadBytes()
        {
            if (!TryLoad(this.byteData))
            {
                this.status = SaveDataStatusType.Failed;
            }

            this.status = SaveDataStatusType.Loaded;
        }

        public void SaveBytes()
        {
            Debug.Assert(this.byteData != null);

            if (!TrySave(out this.byteData))
            {
                this.status = SaveDataStatusType.Failed;
            }

            this.status = SaveDataStatusType.Saved;
        }

        public void LoadString()
        {
            if (!TryLoad(this.stringData))
            {
                this.status = SaveDataStatusType.Failed;
            }

            this.status = SaveDataStatusType.Loaded;
        }

        public void SaveString()
        {
            Debug.Assert(this.stringData != null);

            if (!TrySave(out this.stringData))
            {
                this.status = SaveDataStatusType.Failed;
            }

            this.status = SaveDataStatusType.Saved;
        }

        protected abstract void DoCreateEmpty();
        protected abstract bool TryLoad(byte[] data);
        protected abstract bool TrySave(out byte[] data);
        protected abstract bool TryLoad(string data);
        protected abstract bool TrySave(out string data);
        #endregion
    }
}
