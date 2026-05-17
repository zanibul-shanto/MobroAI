using Microsoft.EntityFrameworkCore;
using MobroLens.Models;

namespace MobroLens;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Child> Children => Set<Child>();
    public DbSet<MeaslesScan> MeaslesScans => Set<MeaslesScan>();
    public DbSet<ScanPhoto> ScanPhotos => Set<ScanPhoto>();
    public DbSet<LocationLog> LocationLogs => Set<LocationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User unique email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure MeaslesScan relationships to avoid multiple cascade paths if needed
        // modelBuilder.Entity<MeaslesScan>()
        //     .HasOne(m => m.UploadedBy)
        //     .WithMany()
        //     .HasForeignKey(m => m.UploadedById)
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}
