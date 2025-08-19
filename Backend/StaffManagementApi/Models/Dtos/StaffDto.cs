namespace StaffManagementApi.Models.Dtos;

public class StaffDto
{
    public string StaffId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateOnly Birthday { get; set; }
    public int Gender { get; set; }  // 1 = Male, 2 = Female
}