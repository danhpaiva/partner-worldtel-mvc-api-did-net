using Microsoft.EntityFrameworkCore;

namespace Partner.WorldTel.Did.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.InternationalDid> InternationalDid { get; set; } = default!;
}
