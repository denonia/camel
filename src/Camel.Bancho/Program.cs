using System.Text.Json.Serialization;
using Camel.Bancho.Middlewares;
using Camel.Bancho.Packets;
using Camel.Bancho.Services;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Interfaces;
using Camel.Core.Performance;
using Camel.Core.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Camel.Bancho;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        if (builder.Environment.IsDevelopment())
            DotEnv.Load(".env.development");
        else
            DotEnv.Load(".env");
        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IUserSessionService, UserSessionService>();
        builder.Services.AddSingleton<IPacketHandlerService, PacketHandlerService>();
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IStatsService, StatsService>();
        builder.Services.AddSingleton<ICryptoService, CryptoService>();
        builder.Services.AddTransient<IScoreService, ScoreService>();
        builder.Services.AddTransient<IBeatmapService, BeatmapService>();
        builder.Services.AddTransient<IPerformanceCalculator, LazerPerformanceCalculator>();
        builder.Services.AddSingleton<ICacheService, CacheService>();
        builder.Services.AddTransient<IRankingService, RedisRankingService>();
        builder.Services.AddSingleton<IChatService, ChatService>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration["REDIS_CONNECTION"]));

        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration["POSTGRES_CONNECTION"])
                .UseSnakeCaseNamingConvention());

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        var domain = app.Configuration["DOMAIN"];
        app.MapAreaControllerRoute("Bancho", "Bancho", "Bancho/Bancho/Index")
            .RequireHost($"c.{domain}", $"ce.{domain}", $"c4.{domain}");
        // app.MapControllerRoute("beatmap", "Beatmap/{action}")
        //     .RequireHost($"b.{domain}");
        // app.MapControllerRoute("web", "Web/{action}")
        //     .RequireHost($"osu.{domain}");
        // app.MapControllerRoute("score", "Score/{action}")
        //     .RequireHost($"osu.{domain}");
        // app.MapControllerRoute("direct", "Direct/{action}")
        //     .RequireHost($"osu.{domain}");
        // app.MapAreaControllerRoute("Avatar", "Avatar", "Avatar/{controller=Avatar}/{action}")
        //     .RequireHost($"a.{domain}");
        // if (app.Environment.IsDevelopment())

        app.UseMiddleware<EnableBufferingMiddleware>();

        app.Run();
    }
}