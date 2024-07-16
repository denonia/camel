using Camel.Bancho.Packets;
using Camel.Bancho.Services;
using Camel.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<UserSessionService, UserSessionService>();
        builder.Services.AddSingleton<PacketHandlerService, PacketHandlerService>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("BanchoDbContext"))
                .UseSnakeCaseNamingConvention());

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // dbContext.Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}