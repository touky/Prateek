namespace Assets.Prateek.ToConvert.SaveGame
{
    using System.Collections.Generic;
    using System.IO;
    using Assets.Prateek.ToConvert.SaveGame.Enums;
    using UnityEngine;

    public class LocalSaveServiceProvider : SaveServiceProvider
    {
        #region Static and Constants
        private static readonly string SAVE_FOLDER_PATH = "SavedGame";
        #endregion

        #region Fields
        private string gameSavePath = string.Empty;
        #endregion

        #region Properties
        public override bool IsProviderValid
        {
            get { return true; }
        }

        public override int Priority
        {
            get { return 0; }
        }
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            gameSavePath = Path.Combine(Application.persistentDataPath, SAVE_FOLDER_PATH);
            CreateSaveFolder(string.Empty);
        }
        #endregion

        #region Class Methods
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<SaveService, SaveServiceProvider>(this);
        }

        private void CreateSaveFolder(string path)
        {
            path = Path.Combine(gameSavePath, path);
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    //DebugTools.LogError($"{GetType().Name}: Can't create folder: {path}");
                }
            }
        }

        private bool TryRead(string path, out byte[] bytes)
        {
            path = Path.Combine(gameSavePath, path);

            bytes = null;
            if (!File.Exists(path))
            {
                return false;
            }

            bytes = File.ReadAllBytes(path);
            return true;
        }

        private bool TryWrite(string path, byte[] bytes)
        {
            path = Path.Combine(gameSavePath, path);

            File.WriteAllBytes(path, bytes);
            return true;
        }

        public override bool TryLoad(IReadOnlyList<SaveDataIdentification> identifications)
        {
            var loadedAll = true;
            foreach (var identification in identifications)
            {
                if (identification.Status == SaveDataStatusType.Loaded
                 || identification.Status == SaveDataStatusType.CleanState)
                {
                    continue;
                }

                if (TryRead(identification.FileName, out var bytes))
                {
                    identification.ByteData = bytes;
                    identification.LoadBytes();
                }

                if (identification.Status == SaveDataStatusType.None
                 || identification.Status == SaveDataStatusType.Failed)
                {
                    loadedAll = false;
                }
            }

            return loadedAll;
        }

        public override bool TrySave(IReadOnlyList<SaveDataIdentification> identifications)
        {
            var savedAll = true;
            foreach (var identification in identifications)
            {
                if (identification.Status == SaveDataStatusType.Saved)
                {
                    continue;
                }

                identification.SaveBytes();
                if (identification.Status == SaveDataStatusType.Failed)
                {
                    savedAll = false;
                    continue;
                }

                if (TryWrite(identification.FileName, identification.ByteData))
                {
                    savedAll = false;
                }
            }

            return savedAll;
        }
        #endregion
    }
}
