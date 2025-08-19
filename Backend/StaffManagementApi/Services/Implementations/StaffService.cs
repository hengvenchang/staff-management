using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using StaffManagementApi.Models.Dtos;
using StaffManagementApi.Data;

namespace StaffManagementApi.Services;

public class StaffService : IStaffService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public StaffService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

        // Required in EPPlus 8+
        ExcelPackage.License.SetNonCommercialPersonal("VenCheang");
    }

    public async Task<IEnumerable<StaffDto>> GetAllAsync()
    {
        var staff = await _context.Staffs.ToListAsync();
        return _mapper.Map<IEnumerable<StaffDto>>(staff);
    }

    public async Task<StaffDto?> GetByIdAsync(string id)
    {
        var staff = await _context.Staffs.FindAsync(id);
        return staff == null ? null : _mapper.Map<StaffDto>(staff);
    }

    public async Task<StaffDto> AddAsync(StaffDto staffDto)
    {
        if (await _context.Staffs.AnyAsync(s => s.StaffId == staffDto.StaffId))
            throw new InvalidOperationException("Staff ID already exists.");

        var staff = _mapper.Map<Staff>(staffDto);
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();
        return _mapper.Map<StaffDto>(staff);
    }

    public async Task<StaffDto> UpdateAsync(string id, StaffDto staffDto)
    {
        var staff = await _context.Staffs.FindAsync(id);
        if (staff == null)
            throw new KeyNotFoundException("Staff not found.");

        _mapper.Map(staffDto, staff);
        await _context.SaveChangesAsync();
        return _mapper.Map<StaffDto>(staff);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var staff = await _context.Staffs.FindAsync(id);
        if (staff == null)
            return false;

        _context.Staffs.Remove(staff);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StaffDto>> SearchAsync(StaffSearchCriteriaDto criteria)
    {
        var query = _context.Staffs.AsQueryable();

        if (!string.IsNullOrEmpty(criteria.StaffId))
            query = query.Where(s => s.StaffId.Contains(criteria.StaffId));
        if (criteria.Gender.HasValue)
            query = query.Where(s => s.Gender == criteria.Gender.Value);
        if (criteria.Name != null)
             query = query.Where(s => s.FullName.Contains(criteria.Name));
        if (criteria.FromDate.HasValue)
            query = query.Where(s => s.Birthday >= criteria.FromDate.Value);
        if (criteria.ToDate.HasValue)
            query = query.Where(s => s.Birthday <= criteria.ToDate.Value);

        var staff = await query.ToListAsync();
        return _mapper.Map<IEnumerable<StaffDto>>(staff);
    }

    public async Task<byte[]> ExportToExcelAsync(StaffSearchCriteriaDto criteria)
    {
        var staff = await SearchAsync(criteria);
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Staff Report");
        worksheet.Cells[1, 1].Value = "Staff ID";
        worksheet.Cells[1, 2].Value = "Full Name";
        worksheet.Cells[1, 3].Value = "Birthday";
        worksheet.Cells[1, 4].Value = "Gender";

        int row = 2;
        foreach (var s in staff)
        {
            worksheet.Cells[row, 1].Value = s.StaffId;
            worksheet.Cells[row, 2].Value = s.FullName;
            worksheet.Cells[row, 3].Value = s.Birthday.ToString("yyyy-MM-dd");
            worksheet.Cells[row, 4].Value = s.Gender == 1 ? "Male" : "Female";
            row++;
        }

        worksheet.Cells.AutoFitColumns();
        return package.GetAsByteArray();
    }

    public async Task<byte[]> ExportToPdfAsync(StaffSearchCriteriaDto criteria)
    {
        var staff = await SearchAsync(criteria);
        using var stream = new MemoryStream();
        using var writer = new PdfWriter(stream);
        using var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        var table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 40, 20, 20 }));
        table.AddHeaderCell("Staff ID");
        table.AddHeaderCell("Full Name");
        table.AddHeaderCell("Birthday");
        table.AddHeaderCell("Gender");

        foreach (var s in staff)
        {
            table.AddCell(s.StaffId);
            table.AddCell(s.FullName);
            table.AddCell(s.Birthday.ToString("yyyy-MM-dd"));
            table.AddCell(s.Gender == 1 ? "Male" : "Female");
        }

        document.Add(table);
        document.Close();
        return stream.ToArray();
    }
}