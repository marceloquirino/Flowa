using OrderAccumulator;
using OrderAccumulator.Fix;
using OrderAccumulator.Fix.IFix;
using OrderAccumulator.Services;
using OrderAccumulator.Services.IServices;
using QuickFix;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IExposureService, ExposureService>();
builder.Services.AddSingleton<FixApplication>();
builder.Services.AddSingleton<IFixServer, FixServer>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var path = config["Fix:ConfigPath"];

    return new SessionSettings(path);
});

Serilog.Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var host = builder.Build();
host.Run();
