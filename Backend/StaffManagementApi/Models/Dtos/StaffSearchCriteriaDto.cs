namespace StaffManagementApi.Models.Dtos;
public class StaffSearchCriteriaDto
{
    public string? StaffId { get; set; }
    public int? Gender { get; set; }
    public string? Name { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
}