namespace Mayfair.Core.Code.SaveGame
{
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.SaveGame.Enums;
    using Mayfair.Core.Code.Utils.Debug;
    using UnityEngine;

    public class LocalSaveDaemonBranch : SaveDaemonBranch
    {
        #region Static and Constants
        private static readonly string SAVE_FOLDER_PATH = "SavedGame";
        #endregion

        #region Fields
        private string gameSavePath = string.Empty;
        #endregion

        #region Unity Methods
        public override void Startup()
        {
            base.Startup();

            this.gameSavePath = Path.Combine(Application.persistentDataPath, SAVE_FOLDER_PATH);
            CreateSaveFolder(string.Empty);
        }
        #endregion

        #region Class Methods
        private void CreateSaveFolder(string path)
        {
            path = Path.Combine(this.gameSavePath, path);
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    DebugTools.LogError($"{GetType().Name}: Can't create folder: {path}");
                }
            }
        }

        private bool TryRead(string path, out byte[] bytes)
        {
            path = Path.Combine(this.gameSavePath, path);

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
            path = Path.Combine(this.gameSavePath, path);

            File.WriteAllBytes(path, bytes);
            return true;
        }

        public override bool TryLoad(IReadOnlyList<SaveDataIdentification> identifications)
        {
            bool loadedAll = true;
            foreach (SaveDataIdentification identification in identifications)
            {
                if (identification.Status == SaveDataStatusType.Loaded
                 || identification.Status == SaveDataStatusType.CleanState)
                {
                    continue;
                }

                if (TryRead(identification.FileName, out byte[] bytes))
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
            bool savedAll = true;
            foreach (SaveDataIdentification identification in identifications)
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
