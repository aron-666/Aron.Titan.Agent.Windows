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
        public string EXE { get; set; }
        public string ServiceName { get; set; }
        public string Discriptions { get; set; }
        public ServiceUtilities(string exe, string serviceName, string discriptions)
        {
            EXE = exe;
            ServiceName = serviceName;
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

        public void InstallService()
        {
            try
            {
                using (var process = new Process())
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "sc",
                        Arguments = $"create \"{ServiceName}\" binPath= \"\\\"{EXE}\\\" --working-dir=%TITAN_AGENT_WORKING_DIR% --server-url=%TITAN_AGENT_SERVER_URL% --key=%TITAN_AGENT_KEY%\" start= auto",
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }

                using (var process = new Process())
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "sc",
                        Arguments = $"description \"{ServiceName}\" \"{Discriptions}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                        CreateNoWindow = true
                    };
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }
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

        public void UninstallService()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"stop \"{ServiceName}\"";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();
                process.Close();

                startInfo.Arguments = $"delete \"{ServiceName}\"";
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
