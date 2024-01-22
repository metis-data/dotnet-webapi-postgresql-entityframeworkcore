using Microsoft.EntityFrameworkCore;

namespace dotnet_webapi_postgresql_entityframeworkcore.Models;

public class ImdbContext : DbContext
{
    public ImdbContext(DbContextOptions<ImdbContext> options)
        : base(options)
    {
    }

    public DbSet<TitleRating> TitleRatings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("imdb")
            .Entity<TitleRating>(
                entity =>
                {
                    entity.HasNoKey();
                });
    }
}