using EquipIT.Api.Models;

namespace EquipIT.Api.Services;

public interface IEquipmentService
{
    Task<IEnumerable<Equipment>> GetAllEquipmentAsync();
    Task BorrowEquipmentAsync(int id, string firstName, string lastName);
    Task ReturnEquipmentAsync(int id);
}
