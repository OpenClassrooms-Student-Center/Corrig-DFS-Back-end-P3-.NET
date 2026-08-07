using EquipIT.Api.Data;
using EquipIT.Api.Models;
using EquipIT.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace EquipIT.Tests;

public class EquipmentServiceTests
{
    private static EquipITDbContext CreateDbContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<EquipITDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new EquipITDbContext(options);
    }

    [Fact]
    public async Task BorrowEquipmentAsync_MarksEquipmentAsUnavailable()
    {
        var context = CreateDbContext(nameof(BorrowEquipmentAsync_MarksEquipmentAsUnavailable));
        context.Equipments.Add(new Equipment { Id = 1, Name = "Monitor", Category = "Display", IsAvailable = true });
        await context.SaveChangesAsync();

        var service = new EquipmentService(context);
        await service.BorrowEquipmentAsync(1, "John", "Doe");

        var equipment = await context.Equipments.FindAsync(1);
        Assert.False(equipment!.IsAvailable);
    }

    [Fact]
    public async Task BorrowEquipmentAsync_RecordsBorrowerDetails()
    {
        var context = CreateDbContext(nameof(BorrowEquipmentAsync_RecordsBorrowerDetails));
        context.Equipments.Add(new Equipment { Id = 1, Name = "Monitor", Category = "Display", IsAvailable = true });
        await context.SaveChangesAsync();

        var service = new EquipmentService(context);
        await service.BorrowEquipmentAsync(1, "John", "Doe");

        var equipment = await context.Equipments.FindAsync(1);
        Assert.Equal("John", equipment!.BorrowerFirstName);
        Assert.Equal("Doe", equipment.BorrowerLastName);
        Assert.NotNull(equipment.BorrowDate);
    }

    [Fact]
    public async Task BorrowEquipmentAsync_ThrowsWhenAlreadyBorrowed()
    {
        var context = CreateDbContext(nameof(BorrowEquipmentAsync_ThrowsWhenAlreadyBorrowed));
        context.Equipments.Add(new Equipment { Id = 1, Name = "Monitor", Category = "Display", IsAvailable = false });
        await context.SaveChangesAsync();

        var service = new EquipmentService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.BorrowEquipmentAsync(1, "John", "Doe"));
    }

    [Fact]
    public async Task ReturnEquipmentAsync_MarksEquipmentAsAvailable()
    {
        var context = CreateDbContext(nameof(ReturnEquipmentAsync_MarksEquipmentAsAvailable));
        context.Equipments.Add(new Equipment
        {
            Id = 1,
            Name = "Monitor",
            Category = "Display",
            IsAvailable = false,
            BorrowerFirstName = "John",
            BorrowerLastName = "Doe",
            BorrowDate = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new EquipmentService(context);
        await service.ReturnEquipmentAsync(1);

        var equipment = await context.Equipments.FindAsync(1);
        Assert.True(equipment!.IsAvailable);
        Assert.NotNull(equipment.LastReturnDate);
        Assert.Null(equipment.BorrowerFirstName);
        Assert.Null(equipment.BorrowerLastName);
    }

    [Fact]
    public async Task ReturnEquipmentAsync_ThrowsWhenNotBorrowed()
    {
        var context = CreateDbContext(nameof(ReturnEquipmentAsync_ThrowsWhenNotBorrowed));
        context.Equipments.Add(new Equipment { Id = 1, Name = "Monitor", Category = "Display", IsAvailable = true });
        await context.SaveChangesAsync();

        var service = new EquipmentService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ReturnEquipmentAsync(1));
    }
}
