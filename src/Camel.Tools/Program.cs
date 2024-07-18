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

        builder.Services.AddTransient<IBeatmapService, BeatmapService>();
        builder.Services.AddTransient<IPerformanceCalculator, LazerPerformanceCalculator>();
        builder.Services.AddHttpClient();

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("BanchoDbContext"))
                .UseSnakeCaseNamingConvention());

        using var host = builder.Build();

        using var serviceScope = host.Services.CreateScope();
        var provider = serviceScope.ServiceProvider;
        var recalculator =
            (PerformanceRecalculator)ActivatorUtilities.CreateInstance(provider, typeof(PerformanceRecalculator));
        recalculator.RunAsync().Wait();
    }
}