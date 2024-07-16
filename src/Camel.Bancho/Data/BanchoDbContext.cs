using Microsoft.EntityFrameworkCore;

namespace Camel.Bancho.Data;

public class BanchoDbContext : DbContext
{
    public BanchoDbContext(DbContextOptions options) : base(options)
    {
    }
}