using System.Text.Json.Serialization;
using Camel.Bancho.Middlewares;
using Camel.Bancho.Packets;
using Camel.Bancho.Services;
using Camel.Bancho.Services.Interfaces;
using Camel.Core.Data;
using Camel.Core.Interfaces;
using Camel.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("BanchoDbContext"))
                .UseSnakeCaseNamingConvention());

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        
        app.UseMiddleware<EnableBufferingMiddleware>(); 

        app.Run();
    }
}