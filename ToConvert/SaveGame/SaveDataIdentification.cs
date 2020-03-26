namespace Assets.Prateek.ToConvert.SaveGame
{
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.SaveGame.Enums;

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
            get { return status; }
        }

        public byte[] ByteData
        {
            get { return byteData; }
            set { byteData = value; }
        }

        public string StringData
        {
            get { return stringData; }
            set { stringData = value; }
        }

        public abstract string FileName { get; }
        #endregion

        #region Class Methods
        public void CreateEmpty()
        {
            status = SaveDataStatusType.CleanState;

            DoCreateEmpty();
        }

        public void LoadBytes()
        {
            if (!TryLoad(byteData))
            {
                status = SaveDataStatusType.Failed;
            }

            status = SaveDataStatusType.Loaded;
        }

        public void SaveBytes()
        {
            Debug.Assert(byteData != null);

            if (!TrySave(out byteData))
            {
                status = SaveDataStatusType.Failed;
            }

            status = SaveDataStatusType.Saved;
        }

        public void LoadString()
        {
            if (!TryLoad(stringData))
            {
                status = SaveDataStatusType.Failed;
            }

            status = SaveDataStatusType.Loaded;
        }

        public void SaveString()
        {
            Debug.Assert(stringData != null);

            if (!TrySave(out stringData))
            {
                status = SaveDataStatusType.Failed;
            }

            status = SaveDataStatusType.Saved;
        }

        protected abstract void DoCreateEmpty();
        protected abstract bool TryLoad(byte[] data);
        protected abstract bool TrySave(out byte[] data);
        protected abstract bool TryLoad(string data);
        protected abstract bool TrySave(out string data);
        #endregion
    }
}
