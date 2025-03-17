using ESIID42025.Models;
using Microsoft.EntityFrameworkCore;

namespace ESIID42025.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurações adicionais do modelo podem ser adicionadas aqui
    }
}