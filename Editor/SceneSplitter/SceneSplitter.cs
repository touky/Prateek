namespace Prateek.SceneSplitter.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [InitializeOnLoad]
    public static class SceneSplitter
    {
        #region Static and Constants
        public const string lineFeed = "{0}\r\n";
        public const string sceneDataFolder = "{0}_unitysplit";
        public const string sceneHeaderExtension = "unityheader";
        public const string scenePartExtension = "unitypart";
        private const int NON_SCENE_UID_LENGTH = 4;
        private static Regex yamlTagDetect = new Regex("--- !u!([0-9]*)\\s*&([0-9]*)");

        private static SceneTracker sceneTracker = new SceneTracker();
        #endregion

        #region Constructors
        static SceneSplitter()
        {
            EditorSceneManager.sceneOpening += OnSceneOpening;
            EditorSceneManager.sceneSaved += OnSceneSaved;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            EditorApplication.quitting += OnQuit;
        }
        #endregion

        #region Class Methods
        private static void OnQuit()
        {
            sceneTracker.Clear(ImportSceneContent);
        }

        private static void OnSceneOpening(string path, OpenSceneMode mode)
        {
            sceneTracker.AddScene(path, mode, ImportSceneContent);
        }

        private static void OnSceneSaved(Scene scene)
        {
            ExportSceneContent(scene.path);
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            sceneTracker.RemoveScene(scene.path, ImportSceneContent);
        }

        private static string GetExportFolder(FileInfo fileInfo)
        {
            return Path.Combine(fileInfo.Directory.FullName, string.Format(sceneDataFolder, Path.GetFileNameWithoutExtension(fileInfo.Name)));
        }

        private static string LoadHeaader(DirectoryInfo directoryInfo)
        {
            return LoadFile(directoryInfo, sceneHeaderExtension);
        }

        private static string LoadParts(DirectoryInfo directoryInfo)
        {
            return LoadFile(directoryInfo, scenePartExtension);
        }

        private static string LoadFile(DirectoryInfo directoryInfo, string extension)
        {
            string content = string.Empty;
            if (!directoryInfo.Exists)
            {
                return content;
            }

            FileInfo[] files = directoryInfo.GetFiles($"*.{extension}");
            foreach (FileInfo file in files)
            {
                content += File.ReadAllText(file.FullName);
            }

            return content;
        }

        private static void ImportSceneContent(string path, bool headerOnly = false)
        {
            FileInfo fileInfo = new FileInfo(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(GetExportFolder(fileInfo));
            string content = string.Empty;

            if (!directoryInfo.Exists)
            {
                return;
            }

            content += LoadHeaader(directoryInfo);
            if (!headerOnly)
            {
                content += LoadParts(directoryInfo);
            }

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            File.WriteAllText(fileInfo.FullName, content);
        }

        private static void ExportSceneContent(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            string[] fileContent = File.ReadAllLines(fileInfo.FullName);

            List<DetectedPart> parts = new List<DetectedPart>();
            parts.Add(new DetectedPart
            {
                content = fileContent,
                startLine = 0,
                match = null
            });

            DetectionObject detector = new DetectionObject
            {
                content = fileContent,
                start = 0,
                parts = parts
            };

            detector.Detect();
            foreach (DetectedPart part in parts)
            {
                part.LoadContent();
            }

            string destinationFolder = GetExportFolder(fileInfo);
            DirectoryInfo dirInfo = new DirectoryInfo(destinationFolder);
            FileInfo[] headerFiles = dirInfo.Exists ? dirInfo.GetFiles($"*.{sceneHeaderExtension}") : new FileInfo[0];
            FileInfo[] partFiles = dirInfo.Exists ? dirInfo.GetFiles($"*.{scenePartExtension}") : new FileInfo[0];

            HashSet<string> files = new HashSet<string>();
            foreach (FileInfo file in headerFiles)
            {
                files.Add(file.Name);
            }

            foreach (FileInfo file in partFiles)
            {
                files.Add(file.Name);
            }

            foreach (DetectedPart part in parts)
            {
                string filename = part.GetFilename(fileInfo.Name);
                if (files.Contains(filename))
                {
                    files.Remove(filename);
                }

                part.Save(destinationFolder, filename);
            }

            foreach (string file in files)
            {
                File.Delete(Path.Combine(destinationFolder, file));
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region Nested type: DetectedPart
        private class DetectedPart
        {
            #region Fields
            public string[] content;
            public Match match;
            public int startLine;
            public string partContent;
            #endregion

            #region Class Methods
            public void LoadContent()
            {
                partContent = string.Empty;
                string line = string.Empty;
                Match match = null;
                do
                {
                    line = content[startLine++];
                    partContent += string.Format(lineFeed, line);

                    if (startLine >= content.Length)
                    {
                        break;
                    }

                    line = content[startLine];
                    match = yamlTagDetect.Match(line);
                } while (!match.Success);
            }

            public string GetFilename(string originalName)
            {
                string filename = string.Empty;
                if (match == null)
                {
                    filename = Path.ChangeExtension(originalName, sceneHeaderExtension);
                }
                else
                {
                    string typeid = match.Groups[match.Groups.Count - 2].Value;
                    string guid = match.Groups[match.Groups.Count - 1].Value;
                    string extension = guid.Length > NON_SCENE_UID_LENGTH ? scenePartExtension : sceneHeaderExtension;
                    filename = $"{Path.GetFileNameWithoutExtension(originalName)}_{guid}_{typeid}.{extension}";
                }

                return filename;
            }

            public void Save(string destination, string filename)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(destination);
                if (!dirInfo.Exists)
                {
                    Directory.CreateDirectory(dirInfo.FullName);
                }

                File.WriteAllText(Path.Combine(destination, filename), partContent);
            }
            #endregion
        }
        #endregion

        #region Nested type: DetectionObject
        private class DetectionObject
        {
            #region Fields
            public string[] content;
            public int start = 0;
            public int stop = int.MaxValue;
            public List<DetectedPart> parts;
            #endregion

            #region Class Methods
            public void Detect()
            {
                stop = Mathf.Min(stop, content.Length);
                for (int i = start; i < stop; i++)
                {
                    string line = content[i];
                    Match match = yamlTagDetect.Match(line);
                    if (match.Success)
                    {
                        AddPart(match, i);
                    }
                }
            }

            private void AddPart(Match match, int index)
            {
                parts.Add(new DetectedPart
                {
                    content = content,
                    match = match,
                    startLine = index
                });
            }
            #endregion
        }
        #endregion

        #region Nested type: SceneTracker
        [Serializable]
        private class SceneTracker
        {
            #region Settings
            [SerializeField]
            private List<string> openedPaths = new List<string>();
            #endregion

            #region Class Methods
            public void AddScene(string path, OpenSceneMode mode, Action<string, bool> importAction)
            {
                if (mode == OpenSceneMode.Single)
                {
                    Clear(importAction);
                }

                openedPaths.Add(path);
                importAction(path, false);

                Log("AddScene");
            }

            public void RemoveScene(string path, Action<string, bool> importAction)
            {
                openedPaths.Remove(path);
                importAction(path, true);

                Log($"RemoveScene: {path}");
            }

            public void Clear(Action<string, bool> importAction)
            {
                for (int p = 0; p < openedPaths.Count; p++)
                {
                    RemoveScene(openedPaths[p--], importAction);
                }
            }

            private void Log(string header)
            {
                string content = $"{header}\r\n";
                foreach (string openedPath in openedPaths)
                {
                    content += $"> {openedPath}\r\n";
                }

                Debug.Log(content);
            }
            #endregion
        }
        #endregion
    }
}
