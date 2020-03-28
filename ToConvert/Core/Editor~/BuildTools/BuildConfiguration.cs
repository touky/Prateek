namespace Mayfair.Core.Editor.BuildTools
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class BuildConfiguration
    {
        public delegate void LogMethod(string msg, params object[] args);

        private LogMethod logMethod;

        public BuildTarget Target { get; }
        public string BuildName { get; }
        public string BuildDirectory { get; }
        public BuildOptions BuildOptions { get; set; }

        public string[] Scenes
        {
            get
            {
                string[] scenes = new string[EditorBuildSettings.scenes.Length];

                for (int i = 0; i < scenes.Length; i++)
                {
                    scenes[i] = EditorBuildSettings.scenes[i].path;
                }

                return scenes;
            }
        }

        public BuildConfiguration(BuildTarget buildTarget, string buildName, string buildDirectory, BuildOptions options, LogMethod logMethod)
        {
            this.logMethod = logMethod;

            Target = buildTarget;
            BuildName = CreateBuildName(buildName);
            BuildDirectory = CreateBuildDirectory(buildDirectory);
            BuildOptions = options;
        }

        private string CreateBuildName(string buildName)
        {
            buildName = string.IsNullOrEmpty(buildName) ? GetProjectName() : buildName;

            if (Target == BuildTarget.Android)
            {
                buildName = string.Concat(buildName, ".apk");
            }

            return buildName;
        }

        private string CreateBuildDirectory(string buildDirectory)
        {
            buildDirectory = string.IsNullOrEmpty(buildDirectory) ? string.Concat(Application.dataPath, "/../") : buildDirectory;

            buildDirectory = Path.Combine(buildDirectory, Enum.GetName(typeof(BuildTarget), Target));

            return buildDirectory;
        }

        private string GetProjectName()
        {
            return Application.productName;
        }
    }
}