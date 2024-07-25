
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Camel.Core.Configuration;

public static class ConfigurationLoader
{
    public static void LoadConfiguration(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
            DotEnv.Load(".env.development");
        else
            DotEnv.Load(".env");

        var exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        builder.Configuration
            .SetBasePath(exeDir)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
            .AddEnvironmentVariables();
    }
}