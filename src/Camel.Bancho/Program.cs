using Camel.Bancho.Data;
using Camel.Bancho.Packets;
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

        builder.Services.AddSingleton<PacketHandlerService, PacketHandlerService>();

        builder.Services.AddDbContext<BanchoDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("BanchoDbContext")));

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BanchoDbContext>();
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

        app.Run();
    }
}
