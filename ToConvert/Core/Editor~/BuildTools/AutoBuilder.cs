namespace Mayfair.Core.Editor.BuildTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BuildInfo;
    using Preprocessing;
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public static class AutoBuilder
    {
        public static void RunBuild(BuildTarget buildTarget, CommandLineParser commandLineParser)
        {
            SetStackTraceLogType(commandLineParser);

            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
            {
                SetPreprocessorDirectives(BuildTarget.Android, commandLineParser); 
            }

            BuildConfiguration buildConfiguration = new BuildConfiguration(buildTarget,
                                                                                  commandLineParser.GetArgument(CommandLineSymbol.BUILD_NAME),
                                                                                  commandLineParser.GetArgument(CommandLineSymbol.BUILD_DIRECTORY),
                                                                                  ParseBuildOptions(commandLineParser.GetArgument(CommandLineSymbol.BUILD_OPTIONS)),
                                                                                  Log);

            BuildInfoUtility.UpdateBuildInfo(commandLineParser.GetArgument(CommandLineSymbol.REVISION));

            BuildProject(buildConfiguration);
            ResetStackTraceLogType();
        }

        public static void Log(string msg, params object[] args)
        {
            msg = string.Format(msg, args);

            string formattedLog = string.Format("  [AutoBuilder] - {0}", msg);
            if (Application.isBatchMode)
            {
                // \033[34m is an ANSI-style color code. I add this to any log in Headless mode
                // to color code the output for easy identification in our CI setup
                formattedLog = string.Format("\\033[34m{0}", formattedLog);
            }

            Debug.LogFormat(formattedLog);
        }

        private static void BuildAndroid()
        {
            CommandLineParser commandLineParser = new CommandLineParser(Log);
            RunBuild(BuildTarget.Android, commandLineParser);
        }

        private static void BuildIos()
        {
            CommandLineParser commandLineParser = new CommandLineParser(Log);
            RunBuild(BuildTarget.iOS, commandLineParser);
        }

        private static void BuildProject(BuildConfiguration buildConfiguration)
        {
            Log("Building for Target: {0}", buildConfiguration.Target);
            Log("Building to directory: {0}", buildConfiguration.BuildDirectory);

            CreateBuildDirectoryIfMissing(buildConfiguration);

            BuildProcessorHelper.RunPreprocessMethods(buildConfiguration.Target);

            string[] scenes = buildConfiguration.Scenes;
            foreach (string scenePath in scenes)
            {
                Debug.Log("Building project with the following scenes:");
                Debug.Log(scenePath);
            }

            BuildReport report = BuildPipeline.BuildPlayer(scenes, CreateBuildPath(buildConfiguration), buildConfiguration.Target, buildConfiguration.BuildOptions);
            
            BuildProcessorHelper.RunPostprocessMethods(report.summary);
        }

        private static void CreateBuildDirectoryIfMissing(BuildConfiguration buildConfiguration)
        {
            if (!Directory.Exists(buildConfiguration.BuildDirectory))
            {
                Log("Directory does not exist, creating it...");
                Directory.CreateDirectory(buildConfiguration.BuildDirectory);
            }
        }

        private static string CreateBuildPath(BuildConfiguration buildConfiguration)
        {
            return Path.Combine(buildConfiguration.BuildDirectory, buildConfiguration.BuildName);
        }

        private static BuildOptions ParseBuildOptions(string buildOptions)
        {
            string[] splitOptions = buildOptions.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            BuildOptions finalBuildOptions = BuildOptions.None;
            foreach (string option in splitOptions)
            {
                if (Enum.TryParse(option, true, out BuildOptions parsedOption))
                {
                    finalBuildOptions |= parsedOption;
                }
            }

            if (finalBuildOptions == BuildOptions.None)
            {
                finalBuildOptions = BuildOptions.Development;
            }

            return finalBuildOptions;
        }

        private static void SetStackTraceLogType(CommandLineParser commandLineParser)
        {
            string diagnosticLevel = commandLineParser.GetArgument(CommandLineSymbol.BUILD_DIAGNOSTIC_LEVEL);
            if (!Enum.TryParse(diagnosticLevel, true, out StackTraceLogType logType))
            {
                logType = StackTraceLogType.ScriptOnly;
            }

            Application.SetStackTraceLogType(LogType.Log, logType);
        }

        private static void SetPreprocessorDirectives(BuildTarget buildTarget, CommandLineParser commandLineParser)
        {
            string preprocessorDirectives = commandLineParser.GetArgument(CommandLineSymbol.PREPROCESSOR_DIRECTIVES);
            string[] splitDirectives = preprocessorDirectives.Split(new []{ ';', ',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            preprocessorDirectives = string.Join(";", splitDirectives);

            Log($"Settings preprocessor directives to {preprocessorDirectives}");

            PlayerSettings.SetScriptingDefineSymbolsForGroup(ConvertToBuildTargetGroup(buildTarget), preprocessorDirectives);
        }

        private static BuildTargetGroup ConvertToBuildTargetGroup(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.Android: return BuildTargetGroup.Android;
                case BuildTarget.iOS: return BuildTargetGroup.iOS;
                default: throw new ArgumentException($"We currently don't support target platform: {buildTarget}");
            }
        }

        private static void ResetStackTraceLogType()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        }
    }
}