using OrderGenerator.Api.Fix;
using OrderGenerator.Api.Fix.IFix;
using OrderGenerator.Api.Services;
using OrderGenerator.Api.Services.IService;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderGeneratorService, OrderGeneratorService>();
builder.Services.AddSingleton<IFixClient, FixClient>();
builder.Services.AddSingleton<IFixMessageBuilder, FixMessageBuilder>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();
var fixClient = app.Services.GetRequiredService<IFixClient>();
fixClient.Start();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
