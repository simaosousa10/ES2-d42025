using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ESIID42025.Models;

namespace ESIID42025.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>  // Usa User do Identity
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Message> Messages { get; set; }
    public DbSet<PriceConfirmation> PriceConfirmations { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Image> Images { get; set; } 
    public DbSet<StoreProd> StoreProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // ⚠ IMPORTANTE: Chamar a base para configurar Identity!

        // Composite Primary Key for StoreProd
        modelBuilder.Entity<StoreProd>()
            .HasKey(sp => new { sp.StoreID, sp.ProductID });

        // Define Foreign Key Relationships with Cascade Delete
        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Price>()
            .HasOne(p => p.User)
            .WithMany(u => u.Prices)
            .HasForeignKey(p => p.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PriceConfirmation>()
            .HasOne(pc => pc.User)
            .WithMany(u => u.PriceConfirmations)
            .HasForeignKey(pc => pc.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Report>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Price>()
            .HasOne(p => p.Store)
            .WithMany(s => s.Prices)
            .HasForeignKey(p => p.StoreID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Price>()
            .HasOne(p => p.Product)
            .WithMany(pr => pr.Prices)
            .HasForeignKey(p => p.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Store>()
            .HasOne(s => s.Report)
            .WithMany(r => r.Stores)
            .HasForeignKey(s => s.ReportID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Report)
            .WithMany(r => r.Products)
            .HasForeignKey(p => p.ReportID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Image>()
            .HasOne(i => i.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
