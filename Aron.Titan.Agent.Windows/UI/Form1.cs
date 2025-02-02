using Hardware.Info;
using MaterialSkin;
using MaterialSkin.Controls;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.IO.Compression;
using System.Management;
using System.Text;
using System.Xml.Linq;

namespace Aron.Titan.Agent.Windows
{
    public partial class Form1 : MaterialForm
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Config.Config _config;
        private readonly MaterialSkinManager materialSkinManager;
        private EnvInfo envInfo;
        private PerformanceCounter? cpuCounter;

        private Task InfoUpdater;
        CancellationTokenSource testCts = new CancellationTokenSource();
        private Task TestTask;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken ct;
        private ServiceUtilities serviceUtilities;
        private string programPath, exePath;


        public bool NeedUpdate
        {
            get
            {
                if (AppVersion == null || LastAppVersion == null)
                {
                    return false;
                }
                // 判斷 AppVersion 是否小於 LastAppVersion
                string[] appVersion = AppVersion.Split('.');
                string[] lastAppVersion = LastAppVersion.Split('.');

                if (appVersion.Length != lastAppVersion.Length)
                {
                    return true;
                }

                for (int i = 0; i < appVersion.Length; i++)
                {

                    if (int.Parse(appVersion[i]) > int.Parse(lastAppVersion[i]))
                    {
                        return false;
                    }
                }
                if (int.Parse(appVersion[appVersion.Length - 1]) == int.Parse(lastAppVersion[appVersion.Length - 1]))
                    return false;
                return true;

            }
        }

        public string? AppVersion { get; private set; }
        public string? LastAppVersion { get; private set; }

        public Form1(IServiceProvider serviceProvider, Config.Config config) : base()
        {
            InitializeComponent();
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            //materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            _serviceProvider = serviceProvider;
            this._config = config;


            ct = cts.Token;
            programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TitanNetwork", "Agent");
            exePath = Path.Combine(programPath, "agent.exe");


            string targetExe = Path.Combine(programPath, "Aron.TitanAgent.WinService.exe");

            serviceUtilities = new ServiceUtilities(targetExe, "AronTitanAgent", "--start-service", "Aron Titan Agent", "Titan Agent Service - By.Aron");




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

            try
            {
                string csprojUrl = "https://raw.githubusercontent.com/aron-666/Aron.Titan.Agent.Windows/master/Aron.Titan.Agent.Windows/Aron.Titan.Agent.Windows.csproj";
                AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                var latestVersion = new HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(10)
                }
                    .GetStringAsync(csprojUrl).GetAwaiter().GetResult();

                LastAppVersion = parseVersion(latestVersion);

                if (NeedUpdate)
                {
                    var res = MaterialMessageBox.Show($"有新版本！ 目前版本: {AppVersion}, 最新版本: {LastAppVersion}，請前往更新。", Text, messageBoxButtons: MessageBoxButtons.YesNo);

                    if (res == DialogResult.Yes)
                    {
                        // get RepositoryUrl from csproj
                        string repositoryUrl = "https://github.com/aron-666/Aron.Titan.Agent.Windows";
                        var psi = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = repositoryUrl,
                            UseShellExecute = true
                        };
                        System.Diagnostics.Process.Start(psi);

                    }
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
        }

        private string parseVersion(string xml)
        {
            try
            {
                // 載入 XML 檔案
                XDocument doc = XDocument.Parse(xml);

                // 找到 PropertyGroup 元素
                XElement propertyGroup = doc.Descendants("PropertyGroup").FirstOrDefault();

                if (propertyGroup != null)
                {
                    // 找到 AssemblyVersion 元素
                    XElement assemblyVersionElement = propertyGroup.Element("AssemblyVersion");

                    if (assemblyVersionElement != null)
                    {
                        // 取得 AssemblyVersion 的值
                        string assemblyVersion = assemblyVersionElement.Value;
                        return assemblyVersion;
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            return null;
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

            string multipassPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Multipass", "bin");
            if (Directory.Exists(multipassPath))
            {
                string paths = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) ?? "";
                if (!paths.Contains(multipassPath))
                {
                    Environment.SetEnvironmentVariable("Path", $"{multipassPath};{paths}", EnvironmentVariableTarget.Machine);



                }

                // 檢查 multipass local dirver 是否設定正確
                // 查看是否在 UI 執行續
                if (!InvokeRequired)
                {
                    string currentDriver = GetMultipassDriver().Trim().ToLower();
                    bool isHyperVEnabled = IsHyperVFeatureEnabled();
                    bool isVirtualBoxInstalled = IsSoftwareInstalled("VirtualBox");
                    // 查看是否安裝 hyperv
                    if (isHyperVEnabled)
                    {
                        if (currentDriver != "hyperv")
                        {
                            var res = MaterialMessageBox.Show($"檢測到您安裝的是 hyperv，但 Multipass 設定是 {currentDriver}，\r\n是否自動修正?", Text, messageBoxButtons: MessageBoxButtons.YesNo);

                            if (res == DialogResult.Yes)
                            {
                                SetMultipassDriver("hyperv");
                            }
                        }
                    }
                    else if (isVirtualBoxInstalled)
                    {
                        if (currentDriver != "virtualbox")
                        {
                            var res = MaterialMessageBox.Show($"檢測到您安裝的是 VirtualBox，但 Multipass 設定是 {currentDriver}，\r\n是否自動修正?", Text, messageBoxButtons: MessageBoxButtons.YesNo);

                            if (res == DialogResult.Yes)
                            {
                                SetMultipassDriver("virtualbox");
                            }
                        }
                    }
                }
            }


        }

        private string GetMultipassDriver()
        {
            string command = "multipass get local.driver";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output;
            }
        }

        private void SetMultipassDriver(string driver)
        {
            string command = $"multipass set local.driver={driver}";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
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
                        string res = serviceUtilities.UninstallService();

                        MaterialMessageBox.Show(res, Text);
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


                    // delete agent-windows.zip
                    System.IO.File.Delete(agentZip);

                    // copy current exe to %ProgramFiles%\TitanNetwork\Agent
                    string currentExe = Path.Combine(Directory.GetCurrentDirectory(), "Service", "Aron.TitanAgent.WinService.exe");
                    string targetExe = Path.Combine(programPath, Path.GetFileName(currentExe));
                    System.IO.File.Copy(currentExe, targetExe, true);

                    string res = serviceUtilities.InstallService();
                    SetMultipassPassphrase(Convert.ToBase64String(Encoding.UTF8.GetBytes(_config.Key ?? "abc")));

                    MaterialMessageBox.Show(res, Text);
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
                    await Task.Delay(5000);

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
                VMStatus = CheckVM(),
                VMCheck = IsVMRunning("ubuntu-niulink"),
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

            lbVMCheck.Text = envInfo.VMCheck ? "Yes" : "No";
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

            SetEnvironmentVariables();
            if (serviceUtilities.IsServiceRunning())
            {
                string ret = serviceUtilities.StopService();
                MaterialMessageBox.Show(ret, Text);
            }
            else
            {
                SetMultipassPassphrase(Convert.ToBase64String(Encoding.UTF8.GetBytes(_config.Key ?? "abc")));
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

            var button = (MaterialButton)sender;
            button.Enabled = false;
            try
            {
                if (envInfo.WinVersion == WinVersion.Windows10Pro)
                {
                    // 詢問 安裝 Hyper-V 或 VirtualBox
                    var res = MaterialMessageBox.Show("安裝Hyper-V 或 VirtualBox?\r\nYes: 安裝Hyper-V\r\nNo: 安裝 VirtualBox ", Text, messageBoxButtons: MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.Yes)
                    {
                        MaterialMessageBox.Show("請手動開啟Hyper-V", Text);
                        Process.Start("optionalfeatures");
                    }
                    else if (res == DialogResult.No)
                    {
                        InstallVirtualBox();
                    }
                }
                else
                {
                    InstallVirtualBox();
                }
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
            finally
            {
                SetEnvironmentVariables();
                Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    button.Invoke(new Action(() =>
                    {
                        button.Enabled = true;
                    }));
                });
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

            var button = (MaterialButton)sender;
            button.Enabled = false;
            try
            {
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
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
            finally
            {
                SetEnvironmentVariables();
                Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    button.Invoke(new Action(() =>
                    {
                        button.Enabled = true;
                    }));
                });
            }

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
                else
                {
                    MaterialMessageBox.Show("目錄不存在", Text);
                }

            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
            if (envInfo == null)
            {
                MaterialMessageBox.Show("請等待環境資訊更新", Text);
                return;
            }

            SetEnvironmentVariables();

            var button = (MaterialButton)sender;
            button.Enabled = false;
            try
            {
                if (!File.Exists(exePath))
                {
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

                    // delete agent-windows.zip
                    System.IO.File.Delete(agentZip);

                }
                string command = $"& \"${{env:ProgramFiles}}\\TitanNetwork\\Agent\\agent.exe\" --working-dir=\"${{env:TITAN_AGENT_WORKING_DIR}}\" --server-url=\"${{env:TITAN_AGENT_SERVER_URL}}\" --key=\"${{env:TITAN_AGENT_KEY}}\"";

                string? workingDir = Environment.GetEnvironmentVariable("TITAN_AGENT_WORKING_DIR", EnvironmentVariableTarget.Machine);
                string? serverUrl = Environment.GetEnvironmentVariable("TITAN_AGENT_SERVER_URL", EnvironmentVariableTarget.Machine);
                string? key = Environment.GetEnvironmentVariable("TITAN_AGENT_KEY", EnvironmentVariableTarget.Machine);
                string? logFile = Environment.GetEnvironmentVariable("LOG_FILE", EnvironmentVariableTarget.Machine);

                if (string.IsNullOrEmpty(workingDir) || string.IsNullOrEmpty(serverUrl) || string.IsNullOrEmpty(key))
                {
                    MaterialMessageBox.Show("請先設定環境變數", Text);
                    return;
                }

                string arguments = $" --working-dir=\"{workingDir}\" --server-url=\"{serverUrl}\" --key=\"{key}\"";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false,
                    WorkingDirectory = programPath,

                };
                startInfo.EnvironmentVariables["LOG_FILE"] = logFile;
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
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
                    await Task.Delay(5000);
                    button.Invoke(new Action(() =>
                    {
                        button.Enabled = true;
                    }));
                });
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (TestTask != null && !TestTask.IsCompleted)
                    throw new Exception("測試還在進行。");

                MaterialMessageBox.Show("按下 Ok 後，測試開始，請等待片刻", Text);
                testCts = new CancellationTokenSource();
                CancellationToken ct = testCts.Token;
                TestTask = Task.Run(() =>
                {
                    try
                    {
                        // 在 multipass 建立 aron-test
                        if(IsVMExist("aron-test"))
                            RemoveVm("aron-test");

                        if (!CreateVm("aron-test"))
                        {
                            throw new Exception("建立 VM 失敗");
                        }

                        // 刪除 aron-test
                        RemoveVm("aron-test");

                        if(InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                MaterialMessageBox.Show("Multipass 測試成功", Text);
                            }));
                        }
                        else
                        {
                            MaterialMessageBox.Show("Multipass 測試成功", Text);
                        }


                    }
                    catch (Exception ex)
                    {
                        if(InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                MaterialMessageBox.Show(ex.Message, Text);
                            }));
                        }
                        else
                        {
                            MaterialMessageBox.Show(ex.Message, Text);
                        }
                    }
                }, ct);
            }
            catch(Exception ex)
            {
                MaterialMessageBox.Show(ex.Message, Text);
            }
        }

        private bool CreateVm(string name)
        {
            string command = $"multipass launch --name {name}";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output.Contains("Launched");
            }
        }

        private bool IsVMExist(string name)
        {
            string command = $"multipass list";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return output.Contains(name);
            }
        }

        private bool IsVMRunning(string name)
        {
            string command = $"multipass list";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    if (line.Contains(name) && line.Contains("Running"))
                    {
                        process.WaitForExit();

                        return true;
                    }
                }
                process.WaitForExit();
                return false;
            }
        }

        private void RemoveVm(string name)
        {
            string command = $"multipass delete --purge {name}";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
        }

        private void SetMultipassPassphrase(string passphrase)
        {
            string command = " set local.passphrase";
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "multipass";
                process.StartInfo.Arguments = $"{command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine(passphrase);
                process.StandardInput.WriteLine(passphrase);
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                

                if (!string.IsNullOrEmpty(error))
                {
                    MaterialMessageBox.Show(error, Text);
                }


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

        public bool VMCheck { get; set; }

    }
}
