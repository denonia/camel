using Camel.Core.Configuration;
using Camel.Core.Data;
using Camel.Core.Entities;
using Camel.Core.Interfaces;
using Camel.Core.Services;
using Camel.Web.Services;
using Camel.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Role = Camel.Core.Entities.Role;

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
        builder.Services.AddTransient<ICommentService, CommentService>();
        builder.Services.AddTransient<IRelationshipService, RelationshipService>();
        
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
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddRazorPages();

        builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            
            if (!dbContext.Users.Any())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var seeder = new DatabaseSeeder(userManager, roleManager);
                seeder.SeedAsync().Wait();
            }
        }

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