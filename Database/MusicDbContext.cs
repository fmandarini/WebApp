using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public class MusicDbContext : DbContext
{
    public DbSet<Concert> Concerts { get; init; } = null!;
    public DbSet<Artist> Artists { get; init; } = null!;
    public MusicDbContext(DbContextOptions<MusicDbContext> dbContextOptions) : base(dbContextOptions) { }
    public MusicDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}