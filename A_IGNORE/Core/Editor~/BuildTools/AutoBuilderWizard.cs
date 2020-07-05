namespace Mayfair.Core.Editor.BuildTools
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class AutoBuilderWizard : ScriptableWizard
    {
        [SerializeField]
        private BuildTarget buildTarget = BuildTarget.Android;

        [SerializeField]
        private BuildOptions buildOptions = BuildOptions.Development;

        [SerializeField]
        private string outputDirectory;

        [SerializeField]
        private string buildName;

        [SerializeField]
        private StackTraceLogType diagnosticLevel = StackTraceLogType.ScriptOnly;

        [SerializeField, Tooltip("In automated builds we use the Revision number as the Build number")]
        private int buildNumber = 1;

        [MenuItem("Tools/AutoBuilder/Run")]
        private static void CreateWizard()
        {
            DisplayWizard<AutoBuilderWizard>("AutoBuilder", "Build");
        }

        private void OnEnable()
        {
            outputDirectory = string.Concat(Application.dataPath, "/../");
            buildName = Application.productName;
        }

        // This method executes when the "Build" button is clicked
        private void OnWizardCreate()
        {
            CommandLineParser commandLineParser = new CommandLineParser(AutoBuilder.Log);

            Dictionary<string, string> arguments = new Dictionary<string, string>
            {
                {
                    CommandLineSymbol.BUILD_DIRECTORY, outputDirectory
                },
                {
                    CommandLineSymbol.BUILD_NAME, buildName
                },
                {
                    CommandLineSymbol.BUILD_OPTIONS, Stringify(buildOptions)
                },
                {
                    CommandLineSymbol.REVISION, buildNumber.ToString()
                },
                {
                    CommandLineSymbol.BUILD_DIAGNOSTIC_LEVEL, diagnosticLevel.ToString()
                }
            };
            commandLineParser.OverrideCommandLineArguments(arguments);

            AutoBuilder.RunBuild(buildTarget, commandLineParser);
        }

        private string Stringify(BuildOptions buildOptions)
        {
            List<BuildOptions> splitOptions = GetActiveOptions(buildOptions);
            return string.Join("|", splitOptions);
        }

        private List<BuildOptions> GetActiveOptions(BuildOptions buildOptions)
        {
            List<BuildOptions> activeOptions = new List<BuildOptions>();

            foreach (Enum value in Enum.GetValues(typeof(BuildOptions)))
            {
                if (buildOptions.HasFlag(value))
                {
                    activeOptions.Add((BuildOptions)value);
                }
            }

            return activeOptions;
        }
    }
}