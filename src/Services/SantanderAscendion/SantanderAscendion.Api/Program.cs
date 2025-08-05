using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SantanderAscendion.Api.Middleware;
using SantanderAscendion.Application;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
string applicationName = AppDomain.CurrentDomain.FriendlyName;

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new ProducesAttribute("application/json"));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = ApiKeyAuthorization.ApiKeyHeader,
        Scheme = "apiKey"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "apiKey",
                    Type = ReferenceType.SecurityScheme,
                },
                In = ParameterLocation.Header
            },
            []
        }
    });
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddSerilog((sp, cl) =>
{
    cl.ReadFrom.Configuration(builder.Configuration)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .ReadFrom.Services(sp)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(
            path: $"logs/{applicationName}_.log",
            outputTemplate: "{Timestamp:HH:mm:ss.fff zzzz} [{Level:u3}] [{RequestId}] {Message:lj}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day);
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddOutputCache(configure =>
{
    configure.AddBasePolicy(policy =>
    {
        policy.Cache();
        policy.Expire(TimeSpan.FromMinutes(1));
    });
});

builder.Services.AddResponseCompression();

var app = builder.Build();
app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(setup =>
{
    setup.DisplayRequestDuration();
});

app.UseOutputCache();
app.UseResponseCompression();
app.UseApiKeyAuthorization();
app.MapControllers();
app.Run();
