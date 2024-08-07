using System.Text.Json.Serialization;
using AspNetCore.Proxy;
using Camel.Bancho.Middlewares;
using Camel.Bancho.Packets;
using Camel.Bancho.Services;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Configuration;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Interfaces;
using Camel.Core.Performance;
using Camel.Core.Services;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Role = Camel.Core.Entities.Role;

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
            ConnectionMultiplexer.Connect(builder.Configuration["REDIS_CONNECTION"] ??
                                          throw new Exception("Redis connection string not set")));

        builder.Services.AddHttpClient();
        builder.Services.AddProxies();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseNpgsql(builder.Configuration["POSTGRES_CONNECTION"] ??
                              throw new Exception("PostgreSQL connection string not set"))
                .UseSnakeCaseNamingConvention();
        });
        
        builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        var app = builder.Build();

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