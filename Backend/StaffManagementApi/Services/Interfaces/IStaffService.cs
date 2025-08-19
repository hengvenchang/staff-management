
using StaffManagementApi.Models.Dtos;

namespace StaffManagementApi.Services;

public interface IStaffService
{
    Task<IEnumerable<StaffDto>> GetAllAsync();
    Task<StaffDto?> GetByIdAsync(string id);
    Task<StaffDto> AddAsync(StaffDto staffDto);
    Task<StaffDto> UpdateAsync(string id, StaffDto staffDto);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<StaffDto>> SearchAsync(StaffSearchCriteriaDto criteria);
    Task<byte[]> ExportToExcelAsync(StaffSearchCriteriaDto criteria);
    Task<byte[]> ExportToPdfAsync(StaffSearchCriteriaDto criteria);
}