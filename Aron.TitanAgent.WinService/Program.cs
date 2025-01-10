using Aron.TitanAgent.WinService;
using Aron.TitanAgent.WinService.Extensions;

string? logDir = Environment.GetEnvironmentVariable("TITAN_AGENT_WORKING_DIR", EnvironmentVariableTarget.Machine);

var builder = Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .AddLog(logDir)
            .ConfigureServices((hostContext, services) =>
            {
                Settings settings = new Settings() { Args = args };
                services.AddSingleton(settings);
                services.AddHostedService<Worker>();
            });

var host = builder.Build();
host.Run();
