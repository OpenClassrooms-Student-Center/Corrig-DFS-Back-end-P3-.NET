namespace EquipIT.Front.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string? BorrowerFirstName { get; set; }
    public string? BorrowerLastName { get; set; }
    public DateTime? BorrowDate { get; set; }
    public DateTime? LastReturnDate { get; set; }
}
