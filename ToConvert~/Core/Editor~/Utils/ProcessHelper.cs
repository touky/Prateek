namespace Mayfair.Core.Editor.Utils
{
    using System.Diagnostics;

    public static class ProcessHelper
    {
        #region Class Methods
        public static Process GetNewProcess(string workingDirectory, string command, string arguments)
        {
            //Prepare process
            Process process = new Process();

            // redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = arguments.Replace("\n", " ");
            process.StartInfo.WorkingDirectory = workingDirectory;

            return process;
        }
        #endregion
    }
}
