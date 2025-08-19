using Microsoft.AspNetCore.Mvc;
using StaffManagementApi.Models.Dtos;
using StaffManagementApi.Services;

namespace StaffManagementApi.Controllers;

[ApiController]
[Route("api/staff")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
    {
        var staff = await _staffService.GetAllAsync();
        return Ok(staff);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StaffDto>> GetById(string id)
    {
        var staff = await _staffService.GetByIdAsync(id);
        if (staff == null)
            return NotFound();
        return Ok(staff);
    }

    [HttpPost]
    public async Task<ActionResult<StaffDto>> Add([FromBody] StaffDto staffDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var created = await _staffService.AddAsync(staffDto);
            return CreatedAtAction(nameof(GetById), new { id = created.StaffId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StaffDto>> Update(string id, [FromBody] StaffDto staffDto)
    {
        if (!ModelState.IsValid || id != staffDto.StaffId)
            return BadRequest();

        try
        {
            var updated = await _staffService.UpdateAsync(id, staffDto);
            return Ok(updated);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var deleted = await _staffService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<StaffDto>>> Search([FromQuery] StaffSearchCriteriaDto criteria)
    {
        var results = await _staffService.SearchAsync(criteria);
        return Ok(results);
    }

    [HttpPost("export/excel")]
    public async Task<IActionResult> ExportToExcel([FromBody] StaffSearchCriteriaDto criteria)
    {
        var fileContent = await _staffService.ExportToExcelAsync(criteria);
        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "staff-report.xlsx");
    }

    [HttpPost("export/pdf")]
    public async Task<IActionResult> ExportToPdf([FromBody] StaffSearchCriteriaDto criteria)
    {
        var fileContent = await _staffService.ExportToPdfAsync(criteria);
        return File(fileContent, "application/pdf", "staff-report.pdf");
    }
}