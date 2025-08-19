using System.ComponentModel.DataAnnotations;

public class Staff
{
    [Key]
    [MaxLength(8)]
    public string StaffId { get; set; } = null!;

    [MaxLength(100)]
    public string FullName { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public int Gender { get; set; }  // 1 = Male, 2 = Female
}