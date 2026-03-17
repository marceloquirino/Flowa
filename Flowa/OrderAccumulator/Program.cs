using OrderAccumulator;
using OrderAccumulator.Fix;
using OrderAccumulator.Fix.IFix;
using OrderAccumulator.Services;
using OrderAccumulator.Services.IServices;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IExposureService, ExposureService>();
builder.Services.AddSingleton<FixApplication>();
builder.Services.AddSingleton<IFixServer, FixServer>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
