using System.Text.Json.Serialization;
using AspNetCore.Proxy;
using Camel.Bancho.Middlewares;
using Camel.Bancho.Packets;
using Camel.Bancho.Services;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Configuration;
using Camel.Core.Data;
using Camel.Core.Interfaces;
using Camel.Core.Performance;
using Camel.Core.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Camel.Bancho;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.LoadConfiguration();

        builder.WebHost.UseKestrel(options =>
        {
            options.ListenAnyIP(13380, o => o.UseConnectionHandler<TcpConnectionHandler>());
            options.ListenAnyIP(13381, o => o.UseConnectionHandler<TcpConnectionHandler>());
            options.ListenAnyIP(13382, o => o.UseConnectionHandler<TcpConnectionHandler>());
            options.ListenAnyIP(13383, o => o.UseConnectionHandler<TcpConnectionHandler>());

            options.ListenAnyIP(8080);
        });

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
        builder.Services.AddTransient<IPerformanceCalculator, ExternalPerformanceCalculator>();
        builder.Services.AddSingleton<ICacheService, CacheService>();
        builder.Services.AddTransient<IRankingService, RedisRankingService>();
        builder.Services.AddSingleton<IChatService, ChatService>();
        builder.Services.AddSingleton<IMultiplayerService, MultiplayerService>();
        builder.Services.AddTransient<IReplayService, ReplayService>();
        builder.Services.AddScoped<IBanchoService, BanchoService>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration["REDIS_CONNECTION"]));

        builder.Services.AddHttpClient();
        builder.Services.AddProxies();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseNpgsql(builder.Configuration["POSTGRES_CONNECTION"])
                .UseSnakeCaseNamingConvention();
        });

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

        app.MapControllers();

        app.UseMiddleware<EnableBufferingMiddleware>();

        app.Run();
    }
}