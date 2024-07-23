using Camel.Core;
using Camel.Core.Configuration;
using Camel.Core.Data;
using Camel.Core.Interfaces;
using Camel.Core.Performance;
using Camel.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Camel.Tools;

class Program
{
    static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        DotEnv.Load(".env.development");
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddTransient<IBeatmapService, BeatmapService>();
        builder.Services.AddTransient<IPerformanceCalculator, ExternalPerformanceCalculator>();
        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration["POSTGRES_CONNECTION"])
                .UseSnakeCaseNamingConvention());

        using var host = builder.Build();

        using var serviceScope = host.Services.CreateScope();
        var provider = serviceScope.ServiceProvider;
        var recalculator =
            (PerformanceRecalculator)ActivatorUtilities.CreateInstance(provider, typeof(PerformanceRecalculator));
        recalculator.RunAsync().Wait();
    }
}