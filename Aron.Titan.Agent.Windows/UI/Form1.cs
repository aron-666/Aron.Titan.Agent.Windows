using Hardware.Info;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Management;
using System.Runtime.InteropServices;

namespace Aron.Titan.Agent.Windows
{
    public partial class Form1 : MaterialSkin.Controls.MaterialForm
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Config.Config _config;
        private readonly MaterialSkinManager materialSkinManager;
        private EnvInfo envInfo;
        private PerformanceCounter? cpuCounter;

        private Task InfoUpdater;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken ct;
        private ServiceUtilities serviceUtilities;
        private string programPath, exePath;


        public Form1(IServiceProvider serviceProvider, Config.Config config) : base()
        {
            InitializeComponent();
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            _serviceProvider = serviceProvider;
            this._config = config;


            ct = cts.Token;
            programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TitanNetwork", "Agent");
            exePath = Path.Combine(programPath, "agent.exe");

            serviceUtilities = new ServiceUtilities(exePath, "Aron Titan Agent", "Titan Agent Service - By.Aron");

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            lbDataDir.Text = _config.DataDir;
            lbKey.Text = _config.Key;
            InfoUpdater = Task.Run(async () =>
            {
                Task t1 = Task.Run(() =>
                {
                    try
                    {
                        cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                        cpuCounter.NextValue();
                    }
                    catch (Exception ex)
                    {
                    }
                });

                Task t2 = Task.Run(() =>
                {
                    try
                    {
                        SetEnvironmentVariables();
                    }
                    catch (Exception ex)
                    {
                    }
                });

                await Task.WhenAll(t1, t2);

                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        envInfo = GetInfo();
                    }
                    catch (Exception ex)
                    {
                    }
                    await Task.Delay(5000);
                }
            }, ct);

        }

        private void SetEnvironmentVariables()
        {
            string currentValue = Environment.GetEnvironmentVariable("TITAN_AGENT_SERVER_URL", EnvironmentVariableTarget.Machine);
            if (currentValue != "https://test4-api.titannet.io")
            {
                Environment.SetEnvironmentVariable("TITAN_AGENT_SERVER_URL", "https://test4-api.titannet.io", EnvironmentVariableTarget.Machine);
            }

            if (!string.IsNullOrEmpty(_config.Key))
            {
                currentValue = Environment.GetEnvironmentVariable("TITAN_AGENT_KEY", EnvironmentVariableTarget.Machine);
                if (currentValue != _config.Key)
                {
                    Environment.SetEnvironmentVariable("TITAN_AGENT_KEY", _config.Key, EnvironmentVariableTarget.Machine);
                }
            }

            if (!string.IsNullOrEmpty(_config.DataDir) && Directory.Exists(_config.DataDir))
            {
                currentValue = Environment.GetEnvironmentVariable("TITAN_AGENT_WORKING_DIR", EnvironmentVariableTarget.Machine);
                if (currentValue != _config.DataDir)
                {
                    Environment.SetEnvironmentVariable("TITAN_AGENT_WORKING_DIR", _config.DataDir, EnvironmentVariableTarget.Machine);
                }

                string logFile = Path.Combine(_config.DataDir, "agent.log");
                currentValue = Environment.GetEnvironmentVariable("LOG_FILE", EnvironmentVariableTarget.Machine);
                if (currentValue != logFile)
                {
                    Environment.SetEnvironmentVariable("LOG_FILE", logFile, EnvironmentVariableTarget.Machine);
                }
            }

            string vBoxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Oracle", "VirtualBox");
            if (Directory.Exists(vBoxPath))
            {
                string paths = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) ?? "";
                if (!paths.Contains(vBoxPath))
                {
                    Environment.SetEnvironmentVariable("Path", $"{vBoxPath};{paths}", EnvironmentVariableTarget.Machine);
                }
            }

        }
        private void btnInstall_Click(object sender, System.EventArgs e)
        {
            if (envInfo == null)
            {
                MaterialMessageBox.Show("請等待環境資訊更新", Text);
                return;
            }

            var button = (MaterialButton)sender;
            button.Enabled = false;
            try
            {
                //if (!envInfo.IsCPUVirtualization)
                //{
                //    MaterialMessageBox.Show("請先開啟CPU虛擬化", Text);
                //    return;
                //}

                if (!envInfo.VMStatus)
                {
                    MaterialMessageBox.Show("請先安裝VM", Text);
                    return;
                }

                if (!CheckMultipass())
                {
                    MaterialMessageBox.Show("請先安裝Multipass", Text);
                    return;
                }

                if (string.IsNullOrEmpty(_config.DataDir))
                {
                    MaterialMessageBox.Show("請先選擇資料目錄", Text);
                    return;
                }

                if (string.IsNullOrEmpty(_config.Key))
                {
                    MaterialMessageBox.Show("請先輸入身分碼，並儲存", Text);
                    return;
                }




                if (serviceUtilities.IsInstalledService())
                {
                    // yes no message box
                    if (MaterialMessageBox.Show("是否卸載？", Text, messageBoxButtons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        serviceUtilities.UninstallService();
                    }
                }
                else
                {
                    // mkdir -p %ProgramFiles%\TitanNetwork\Agent
                    if (!System.IO.Directory.Exists(programPath))
                    {
                        System.IO.Directory.CreateDirectory(programPath);
                    }

                    // download agent.exe from https://pcdn.titannet.io/test4/bin/agent-windows.zip

                    string agentZip = Path.Combine(programPath, "agent-windows.zip");
                    HttpClient client = new HttpClient();
                    var resp = client.GetAsync("https://pcdn.titannet.io/test4/bin/agent-windows.zip").GetAwaiter().GetResult();
                    using (var fs = new FileStream(
                        agentZip,
                        FileMode.Create, // 使用 FileMode.Create 來覆蓋已存在的檔案
                        FileAccess.Write,
                        FileShare.None))
                    {
                        resp.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                    }

                    // unzip agent-windows.zip
                    ZipFile.ExtractToDirectory(agentZip, programPath, true);

                    serviceUtilities.InstallService();

                    // delete agent-windows.zip
                    System.IO.File.Delete(agentZip);

                    MaterialMessageBox.Show("安裝成功", Text);
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
            finally
            {
                Task.Run(async () =>
                {
                    await Task.Delay(10000);

                    button.Invoke(new Action(() =>
                    {
                        button.Enabled = true;
                    }));
                });
            }

        }
        private void btnSelectDir_Click(object sender, System.EventArgs e)
        {
            // Select directory
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbDataDir.Text = folderBrowserDialog1.SelectedPath;
                _config.DataDir = folderBrowserDialog1.SelectedPath;
                _config.Save();
                SetEnvironmentVariables();
            }
        }

        private void btnSaveKey_Click(object sender, EventArgs e)
        {

            try
            {
                _config.Key = lbKey.Text;
                _config.Save();
                SetEnvironmentVariables();

                MaterialMessageBox.Show("已儲存", Text);
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
        }

        private void btnGoToDashboard_Click(object sender, System.EventArgs e)
        {
            // Go to dashboard
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://test4.titannet.io/walletManagement",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }

        private void lbDataDir_DoubleClick(object sender, System.EventArgs e)
        {
            // Open directory
            Process.Start("explorer.exe", _config.DataDir);
        }

        private WinVersion GetWinVersion()
        {
            // Get Windows version
            string result = string.Empty;
            var reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (reg != null)
            {
                result = reg.GetValue("ProductName").ToString();
            }
            if (result.Contains("Windows 10 Pro"))
            {
                return WinVersion.Windows10Pro;
            }
            else if (result.Contains("Windows 10 Home"))
            {
                return WinVersion.Windows10Basic;
            }
            else
            {
                return WinVersion.Else;
            }

        }

        public EnvInfo GetInfo()
        {
            // Get environment information
            var computerInfo = new ComputerInfo();
            var driveInfo = new System.IO.DriveInfo(System.IO.Path.GetPathRoot(string.IsNullOrEmpty(_config.DataDir) ? "C:\\" : _config.DataDir));

            float? cpuUsage = cpuCounter?.NextValue();

            return new EnvInfo
            {
                WinVersion = GetWinVersion(),
                TotalCPUs = System.Environment.ProcessorCount,
                AvailableCPUs = (int)(System.Environment.ProcessorCount * (1 - (cpuUsage ?? 0) * 0.01)),
                TotalMemoryGB = (decimal)(computerInfo.TotalPhysicalMemory / (decimal)(1024 * 1024 * 1024)),
                AvailableMemoryGB = (decimal)(computerInfo.AvailablePhysicalMemory / (decimal)(1024 * 1024 * 1024)),
                TotalDiskGB = (decimal)(driveInfo.TotalSize / (decimal)(1024 * 1024 * 1024)),
                AvailableDiskGB = (decimal)(driveInfo.AvailableFreeSpace / (decimal)(1024 * 1024 * 1024)),
                IsCPUVirtualization = CPUVirtualization(),
                CpuUsagePercentage = cpuUsage,
                VMStatus = CheckVM()
            };
        }

        private void UpdateInfo()
        {
            if (envInfo == null)
            {
                return;
            }
            lbWinVersion.Text = envInfo.WinVersion?.ToString();
            lbVirtualization.Text = envInfo.IsCPUVirtualization ? "Yes" : "No";
            lbCPU.Text = $"{envInfo.AvailableCPUs}/{envInfo.TotalCPUs}  ({envInfo.CpuUsagePercentage?.ToString("F0")}%)";
            lbMem.Text = $"{envInfo.AvailableMemoryGB?.ToString("F2")}/{envInfo.TotalMemoryGB?.ToString("F2")}  ({envInfo.AvailableMemoryGB / envInfo.TotalMemoryGB * 100:F0}%)";
            lbDisk.Text = $"{envInfo.AvailableDiskGB?.ToString("F2")}/{envInfo.TotalDiskGB?.ToString("F2")}  ({envInfo.AvailableDiskGB / envInfo.TotalDiskGB * 100:F0}%)";
            lbVm.Text = envInfo.VMStatus ? "Yes" : "No";
            lbMultipassStatus.Text = CheckMultipass() ? "Yes" : "No";
        }

        public bool CPUVirtualization()
        {
            bool vmxSupported = IsVirtualizationSupported();
            if (vmxSupported)
            {
                return true;
            }
            using (var searcher = new ManagementObjectSearcher("select * from Win32_Processor"))
            {
                foreach (var item in searcher.Get())
                {
                    if (item["VirtualizationFirmwareEnabled"] != null)
                    {
                        return (bool)item["VirtualizationFirmwareEnabled"];
                    }
                }
            }
            return false;
        }

        public bool IsVirtualizationSupported()
        {
            var hardwareInfo = new HardwareInfo();
            hardwareInfo.RefreshCPUList();

            var cpu = hardwareInfo.CpuList.FirstOrDefault();
            return cpu?.VirtualizationFirmwareEnabled ?? false;
        }



        public bool CheckVM()
        {
            //if (!CPUVirtualization())
            //{
            //    return false;
            //}

            return IsHyperVFeatureEnabled() || IsSoftwareInstalled("VirtualBox");
            //if (GetWinVersion() == WinVersion.Windows10Pro)
            //{
            //    // 確認是否安裝 Hyper-V
            //    return IsSoftwareInstalled("Hyper-V");
            //}
            //else
            //{
            //    // 確認是否安裝 VirtualBox
            //    return IsSoftwareInstalled("Oracle VM VirtualBox");
            //}

        }

        public bool CheckMultipass()
        {
            return IsSoftwareInstalled("Multipass");
        }

        private bool IsSoftwareInstalled(string softwareName)
        {
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    foreach (var subkeyName in key.GetSubKeyNames())
                    {
                        using (var subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey != null && subkey.GetValue("DisplayName") != null)
                            {
                                if (subkey.GetValue("DisplayName").ToString().Contains(softwareName))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            // Also check the 32-bit registry view on 64-bit systems
            registryKey = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    foreach (var subkeyName in key.GetSubKeyNames())
                    {
                        using (var subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey != null && subkey.GetValue("DisplayName") != null)
                            {
                                if (subkey.GetValue("DisplayName").ToString().Contains(softwareName))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsHyperVFeatureEnabled()
        {
            string command = "Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V | Select-Object -ExpandProperty State";

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $"-Command \"{command}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                return output.Equals("Enabled", StringComparison.OrdinalIgnoreCase);
            }
        }


        private void timer1_Tick(object sender, System.EventArgs e)
        {
            try
            {
                UpdateInfo();

                if (!serviceUtilities.IsInstalledService())
                {
                    lbServiceStatus.Text = "未安裝";
                    if (btnInstall.Text != "安裝服務")
                    {
                        btnInstall.Text = "安裝服務";
                    }
                }
                else
                {
                    if (btnInstall.Text != "卸載服務")
                    {
                        btnInstall.Text = "卸載服務";
                    }
                    if (serviceUtilities.IsServiceRunning())
                    {
                        if (btnStart.Text != "停止服務")
                        {
                            btnStart.Text = "停止服務";
                        }
                        lbServiceStatus.Text = "運行中";
                    }
                    else
                    {
                        if (btnStart.Text != "啟動服務")
                        {
                            btnStart.Text = "啟動服務";
                        }
                        lbServiceStatus.Text = "未運行";
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cts?.Cancel();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (envInfo == null)
            {
                MaterialMessageBox.Show("請等待環境資訊更新", Text);
                return;
            }

            if (!serviceUtilities.IsInstalledService())
            {
                MaterialMessageBox.Show("請先安裝服務", Text);
                return;
            }

            //if (!envInfo.IsCPUVirtualization)
            //{
            //    MaterialMessageBox.Show("請先開啟CPU虛擬化", Text);
            //    return;
            //}

            if (!envInfo.VMStatus)
            {
                MaterialMessageBox.Show("請先安裝VM", Text);
                return;
            }

            if (!CheckMultipass())
            {
                MaterialMessageBox.Show("請先安裝Multipass", Text);
                return;
            }

            if (string.IsNullOrEmpty(_config.DataDir))
            {
                MaterialMessageBox.Show("請先選擇資料目錄", Text);
                return;
            }

            if (string.IsNullOrEmpty(_config.Key))
            {
                MaterialMessageBox.Show("請先輸入身分碼，並儲存", Text);
                return;
            }
            if (serviceUtilities.IsServiceRunning())
            {
                string ret = serviceUtilities.StopService();
                MaterialMessageBox.Show(ret, Text);
            }
            else
            {
                string ret = serviceUtilities.StartService();
                MaterialMessageBox.Show(ret, Text);
            }
        }

        private void btnInstallVM_Click(object sender, EventArgs e)
        {
            if (envInfo == null)
            {
                MaterialMessageBox.Show("請等待環境資訊更新", Text);
                return;
            }

            if (envInfo.VMStatus)
            {
                MaterialMessageBox.Show("已安裝VM", Text);
                return;
            }

            if (envInfo.WinVersion == WinVersion.Windows10Pro)
            {
                // 詢問 安裝 Hyper-V 或 VirtualBox
                if (MaterialMessageBox.Show("安裝Hyper-V 或 VirtualBox?\r\nYes: 安裝Hyper-V\r\nNo: 安裝 VirtualBox ", Text, messageBoxButtons: MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    MaterialMessageBox.Show("請手動開啟Hyper-V", Text);
                    Process.Start("optionalfeatures");
                }
                else
                {
                    InstallVirtualBox();
                }
            }
            else
            {
                InstallVirtualBox();
            }
        }

        private void InstallVirtualBox()
        {
            // 安裝 VirtualBox
            // 建立 temp 目錄
            string tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            // 下載 VirtualBox
            // https://download.virtualbox.org/virtualbox/7.1.4/VirtualBox-7.1.4-165100-Win.exe
            string virtualBoxExe = Path.Combine(tempDir, "VirtualBox-Win.exe");
            HttpClient client = new HttpClient();
            var resp = client.GetAsync("https://download.virtualbox.org/virtualbox/7.1.4/VirtualBox-7.1.4-165100-Win.exe").GetAwaiter().GetResult();
            using (var fs = new FileStream(
                virtualBoxExe,
                FileMode.Create, // 使用 FileMode.Create 來覆蓋已存在的檔案
                FileAccess.Write,
                FileShare.None))
            {
                resp.Content.CopyToAsync(fs).GetAwaiter().GetResult();
            }

            // 安裝 VirtualBox
            Process process = Process.Start(virtualBoxExe);

            // 等待安裝完成
            process.WaitForExit();

            // 刪除 temp 目錄
            Directory.Delete(tempDir, true);
        }

        private void btnInstallMultipass_Click(object sender, EventArgs e)
        {
            if (envInfo == null)
            {
                MaterialMessageBox.Show("請等待環境資訊更新", Text);
                return;
            }

            if (IsHyperVFeatureEnabled())
            {
                MaterialMessageBox.Show("等下安裝記得選擇Hyper-V", Text);
            }
            else if (IsSoftwareInstalled("VirtualBox"))
            {
                MaterialMessageBox.Show("等下安裝記得選擇VirtualBox", Text);
            }

            if (CheckMultipass())
            {
                MaterialMessageBox.Show("已安裝Multipass", Text);
                return;
            }

            // 安裝 Multipass
            // https://nextcloud.aronhome.com/apps/sharingpath/Aron/public/multipass-win64.msi
            string tempDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string multipassmsi = Path.Combine(tempDir, "multipass.msi");
            HttpClient client = new HttpClient();
            var resp = client.GetAsync("https://nextcloud.aronhome.com/apps/sharingpath/Aron/public/multipass-win64.msi").GetAwaiter().GetResult();
            using (var fs = new FileStream(
                multipassmsi,
                FileMode.Create, // 使用 FileMode.Create 來覆蓋已存在的檔案
                FileAccess.Write,
                FileShare.None))
            {
                resp.Content.CopyToAsync(fs).GetAwaiter().GetResult();
            }

            Process process = Process.Start("msiexec", $"/i \"{multipassmsi}\"");
            process.WaitForExit();
            Directory.Delete(tempDir, true);


        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(_config.DataDir))
                {
                    // open explorer
                    Process.Start("explorer.exe", _config.DataDir);
                }

            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
            // 使用 PowerShell 來啟動應用程式
            string command = $"& \"${{env:ProgramFiles}}\\TitanNetwork\\Agent\\agent.exe\" --working-dir=\"${{env:TITAN_AGENT_WORKING_DIR}}\" --server-url=\"${{env:TITAN_AGENT_SERVER_URL}}\" --key=\"${{env:TITAN_AGENT_KEY}}\"";
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -NoExit -Command \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = false,
                WorkingDirectory = programPath
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
            }


        }
    }

    public enum WinVersion
    {
        Windows10Pro,
        Windows10Basic,
        Else
    }

    public class EnvInfo
    {
        public WinVersion? WinVersion { get; set; }
        public int? TotalCPUs { get; set; }
        public int? AvailableCPUs { get; set; }
        public decimal? TotalMemoryGB { get; set; }
        public decimal? AvailableMemoryGB { get; set; }
        public decimal? TotalDiskGB { get; set; }
        public decimal? AvailableDiskGB { get; set; }
        public bool IsCPUVirtualization { get; set; }

        public float? CpuUsagePercentage { get; set; }

        public bool VMStatus { get; set; }

    }
}
