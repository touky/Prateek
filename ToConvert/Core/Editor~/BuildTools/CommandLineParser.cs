namespace Mayfair.Core.Editor.BuildTools
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CommandLineParser
    {
        public delegate void LogMethod(string msg, params object[] args);

        private Dictionary<string, string> commandLineArguments;
        private LogMethod Logger { get; }

        private CommandLineParser() { }

        public CommandLineParser(LogMethod logger)
        {
            this.Logger = logger;
            commandLineArguments = GetCommandLineArguments();
        }

        public void OverrideCommandLineArguments(Dictionary<string, string> arguments)
        {
            commandLineArguments = arguments;
            LogArguments(arguments);
        }

        public string GetArgument(string argument)
        {
            string value;

            if (commandLineArguments != null)
            {
                if (commandLineArguments.TryGetValue(argument, out value))
                {
                    return value;
                }
            }

            Logger("No {0} argument detected", argument);
            value = string.Empty;
            return value;
        }

        private Dictionary<string, string> GetCommandLineArguments()
        {
            string[] allArguments = Environment.GetCommandLineArgs();
            Dictionary<string, string> parsedArguments = ConstructArgumentsDictionary(allArguments);
            LogArguments(parsedArguments);

            return parsedArguments;
        }

        private Dictionary<string, string> ConstructArgumentsDictionary(string[] argumentStrings)
        {
            const string KEY_SYMBOL = "-";

            Dictionary<string, string> commandLineArguments = new Dictionary<string, string>();
            for (int i = 0; i < argumentStrings.Length; i++)
            {
                if (argumentStrings[i].Contains(KEY_SYMBOL))
                {
                    string key = argumentStrings[i].Trim();

                    int nextValueIndex = i + 1;
                    if (nextValueIndex >= argumentStrings.Length)
                    {
                        break;
                    }

                    string value = argumentStrings[nextValueIndex].Trim();

                    // Sometimes the key is a configuration flag and it has no corresponding value
                    if (value.Contains(KEY_SYMBOL))
                    {
                        value = string.Empty;
                    }

                    commandLineArguments.Add(key, value);
                }
            }

            return commandLineArguments;
        }

        private void LogArguments(Dictionary<string, string> commandLineArguments)
        {
            Logger("The following command line arguments have been detected:");

            foreach (KeyValuePair<string, string> argumentPair in commandLineArguments)
            {
                Logger("{0} : {1}", argumentPair.Key, argumentPair.Value);
            }
        }
    }
}