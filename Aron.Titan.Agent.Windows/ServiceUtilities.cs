using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aron.Titan.Agent.Windows
{
    public class ServiceUtilities
    {
        private string Args { get; }
        public string DisplayName { get; }

        public string EXE { get; }
        public string ServiceName { get; }
        public string Discriptions { get; }
        public ServiceUtilities(string exe, string serviceName, string args, string displayName, string discriptions)
        {
            EXE = exe;
            ServiceName = serviceName;
            this.Args = args;
            this.DisplayName = displayName;
            Discriptions = discriptions;
        }
        public bool IsInstalledService()
        {
            try
            {
                // sc query GrassMiner
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"query \"{ServiceName}\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                return output.Contains($"SERVICE_NAME: {ServiceName}");
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsServiceRunning()
        {
            try
            {
                // sc query GrassMiner
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"query \"{ServiceName}\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                return output.Contains("RUNNING");
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string InstallService()
        {
            try
            {
                string output = "";
                using (var process = new Process())
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "sc",
                        Arguments = $"create \"{ServiceName}\" binPath= \"\\\"{EXE}\\\" {Args} \" start= auto DisplayName= \"{(string.IsNullOrEmpty(DisplayName) ? ServiceName : DisplayName)}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    output = process.StandardOutput.ReadToEnd();
                    output += "\r\n\r\n" + process.StandardError.ReadToEnd();
                }

                using (var process = new Process())
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "sc",
                        Arguments = $"description \"{ServiceName}\" \"{Discriptions}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }

                return output;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string StartService()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"start \"{ServiceName}\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                output += "\r\n\r\n" + process.StandardError.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public string StopService()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"stop \"{ServiceName}\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                output += "\r\n\r\n" + process.StandardError.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public string UninstallService()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"stop \"{ServiceName}\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();
                process.Close();

                startInfo.Arguments = $"delete \"{ServiceName}\"";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                output += "\r\n\r\n" + process.StandardError.ReadToEnd();
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
