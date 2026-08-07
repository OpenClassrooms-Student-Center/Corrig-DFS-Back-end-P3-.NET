using EquipIT.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipIT.Api.Data;

public class EquipITDbContext : DbContext
{
    public EquipITDbContext(DbContextOptions<EquipITDbContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipments => Set<Equipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipment>().HasData(
            new Equipment { Id = 1, Name = "Monitor", Category = "Display", IsAvailable = true },
            new Equipment { Id = 2, Name = "Keyboard", Category = "Input", IsAvailable = true },
            new Equipment { Id = 3, Name = "Mouse", Category = "Input", IsAvailable = true }
        );
    }
}
