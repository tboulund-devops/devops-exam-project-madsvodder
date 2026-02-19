using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

    public DbSet<User> Users => Set<User>();
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(m => m.Year).IsRequired();
            entity.Property(m => m.Rating).IsRequired();

        });
    }
}