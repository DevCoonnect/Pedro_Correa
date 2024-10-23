using AuthMuseum.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthMuseum.Infra.Database;

public class PostgresDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Art> Arts { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Permission> Permissions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.IndividualPermissions)
            .WithMany()
            .UsingEntity(e => e.ToTable("user_permissions"));

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Profile)
            .HasConversion<string>();
        
        modelBuilder
            .Entity<Permission>()
            .Property(p => p.Value)
            .HasConversion<string>();
    }
}