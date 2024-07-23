using Camel.Core.Configuration;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Interfaces;
using Camel.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Camel.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.LoadConfiguration();
        
        builder.Services.AddTransient<IPasswordHasher<User>, MD5PasswordHasher>();
        builder.Services.AddTransient<Services.Interfaces.IScoreService, Services.ScoreService>();
        builder.Services.AddTransient<IBeatmapService, BeatmapService>();
        builder.Services.AddTransient<IRankingService, RedisRankingService>();
        
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration["REDIS_CONNECTION"] ?? 
                                          throw new InvalidOperationException("Redis connection string not set")));
        
        builder.Services.AddHttpClient();

        // Add services to the container.
        var connectionString = builder.Configuration["POSTGRES_CONNECTION"] ??
                               throw new InvalidOperationException("PostgreSQL connection string not set");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddRazorPages();

        builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}