namespace Mayfair.Core.Editor.EditorWindows
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Editor.Utils;

    public class CommandLineEditorWindow : BaseProcessEditorWindow
    {
        #region Fields
        private List<Process> activeProcess = new List<Process>();
        private string processOutput = string.Empty;
        #endregion

        #region Process Execution
        protected override void PreExecuteProcess()
        {
            base.PreExecuteProcess();

            this.processOutput = string.Empty;
        }

        protected override bool ExecuteProcess(int pass)
        {
            base.ExecuteProcess(pass);

            string processFolder = string.Empty;
            GetFieldSlotValues(FieldSlot.ProcessFolder, ref processFolder);

            //Prepare process
            Process process = ProcessHelper.GetNewProcess(processFolder, this.cmdLineFileName, this.cmdLineArguments);

            try
            {
                process.OutputDataReceived += ProcessOutput;
                process.ErrorDataReceived += ProcessError;

                if (!process.Start())
                {
                    process = null;
                }
                else
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    this.activeProcess.Add(process);
                }
            }
            catch (Exception e)
            {
                this.logger.Log("Run error " + e);
            }

            return true;
        }

        private void ProcessOutput(object s, DataReceivedEventArgs ea)
        {
            if (ea.Data != null)
            {
                this.processOutput += ea.Data;
            }
        }

        private void ProcessError(object s, DataReceivedEventArgs ea)
        {
            if (ea.Data != null)
            {
                this.processOutput += string.Format("{0} {1}\n", LOG_ERROR.value, ea.Data);
            }
        }

        protected override bool WaitForAdditionalProcessing()
        {
            base.WaitForAdditionalProcessing();

            if (this.processOutput != string.Empty)
            {
                string[] split = this.processOutput.Replace('\r', ' ').Split('\n');
                this.logger.Log(split);
            }

            for (int p = 0; p < this.activeProcess.Count; p++)
            {
                Process process = this.activeProcess[p];
                if (process.HasExited)
                {
                    this.exitCode = process.ExitCode;

                    process.OutputDataReceived -= ProcessOutput;
                    process.ErrorDataReceived -= ProcessError;
                    process.Dispose();

                    if (this.exitCode == 0)
                    {
                        this.logger.Log(LOG_TITLE, $" PROCESS {p} ENDED WITH SUCCESS");
                    }
                    else
                    {
                        this.logger.Log(LOG_ERROR, $" PROCESS {p} ENDED WITH ERROR");
                    }

                    this.activeProcess.RemoveAt(p--);
                }
            }

            return this.activeProcess.Count == 0;
        }
        #endregion Process Execution
    }
}
