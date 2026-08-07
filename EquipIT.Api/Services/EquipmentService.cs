using EquipIT.Api.Data;
using EquipIT.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipIT.Api.Services;

public class EquipmentService : IEquipmentService
{
    private readonly EquipITDbContext _context;

    public EquipmentService(EquipITDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
    {
        return await _context.Equipments.ToListAsync();
    }

    public async Task BorrowEquipmentAsync(int id, string firstName, string lastName)
    {
        var equipment = await FindEquipmentAsync(id);
        if (!equipment.IsAvailable)
        {
            throw new InvalidOperationException($"Equipment '{equipment.Name}' is already borrowed.");
        }

        equipment.IsAvailable = false;
        equipment.BorrowerFirstName = firstName;
        equipment.BorrowerLastName = lastName;
        equipment.BorrowDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task ReturnEquipmentAsync(int id)
    {
        var equipment = await FindEquipmentAsync(id);
        if (equipment.IsAvailable)
        {
            throw new InvalidOperationException($"Equipment '{equipment.Name}' is not currently borrowed.");
        }

        equipment.IsAvailable = true;
        equipment.LastReturnDate = DateTime.UtcNow;
        equipment.BorrowerFirstName = null;
        equipment.BorrowerLastName = null;
        equipment.BorrowDate = null;
        await _context.SaveChangesAsync();
    }

    private async Task<Equipment> FindEquipmentAsync(int id)
    {
        var equipment = await _context.Equipments.FindAsync(id);
        if (equipment is null)
        {
            throw new InvalidOperationException($"Equipment with id {id} was not found.");
        }

        return equipment;
    }
}
