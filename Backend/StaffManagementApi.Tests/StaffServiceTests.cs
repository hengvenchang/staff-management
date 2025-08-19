using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StaffManagementApi.Data;
using StaffManagementApi.Models.Dtos;
using StaffManagementApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StaffManagementApi.Tests;

public class StaffServiceTests
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly StaffService _service;

    public StaffServiceTests()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        // Set up AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Staff, StaffDto>().ReverseMap();
            cfg.CreateMap<StaffSearchCriteriaDto, StaffSearchCriteriaDto>();
        });
        _mapper = mapperConfig.CreateMapper();

        _service = new StaffService(_context, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllStaff()
    {
        // Arrange
        await _context.Staffs.AddRangeAsync(
            new Staff { StaffId = "1", FullName = "John", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 },
            new Staff { StaffId = "2", FullName = "Jane", Birthday = DateOnly.Parse("1995-01-01"), Gender = 2 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, s => s.StaffId == "1" && s.FullName == "John");
        Assert.Contains(result, s => s.StaffId == "2" && s.FullName == "Jane");
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsStaff()
    {
        // Arrange
        var staff = new Staff { StaffId = "1", FullName = "John", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 };
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.StaffId);
        Assert.Equal("John", result.FullName);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        // Act
        var result = await _service.GetByIdAsync("999");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ValidStaff_ReturnsStaffDto()
    {
        // Arrange
        var staffDto = new StaffDto
        {
            StaffId = "1",
            FullName = "John Doe",
            Birthday = DateOnly.Parse("1990-01-01"),
            Gender = 1
        };

        // Act
        var result = await _service.AddAsync(staffDto);

        // Assert
        Assert.Equal("1", result.StaffId);
        Assert.Equal(staffDto.FullName, result.FullName);
        Assert.Equal(staffDto.Birthday, result.Birthday);
        Assert.Equal(staffDto.Gender, result.Gender);

        var staffInDb = await _context.Staffs.FindAsync("1");
        Assert.NotNull(staffInDb);
        Assert.Equal(staffDto.FullName, staffInDb.FullName);
    }

    [Fact]
    public async Task AddAsync_DuplicateStaffId_ThrowsException()
    {
        // Arrange
        await _context.Staffs.AddAsync(new Staff { StaffId = "1", FullName = "John", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 });
        await _context.SaveChangesAsync();

        var staffDto = new StaffDto
        {
            StaffId = "1",
            FullName = "Jane",
            Birthday = DateOnly.Parse("1995-01-01"),
            Gender = 2
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(staffDto));
    }

    [Fact]
    public async Task UpdateAsync_ValidStaff_UpdatesSuccessfully()
    {
        // Arrange
        var staff = new Staff { StaffId = "1", FullName = "John", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 };
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();

        var staffDto = new StaffDto
        {
            StaffId = "1",
            FullName = "John Updated",
            Birthday = DateOnly.Parse("1991-01-01"),
            Gender = 2
        };

        // Act
        var result = await _service.UpdateAsync("1", staffDto);

        // Assert
        Assert.Equal("John Updated", result.FullName);
        Assert.Equal(DateOnly.Parse("1991-01-01"), result.Birthday);
        Assert.Equal(2, result.Gender);

        var staffInDb = await _context.Staffs.FindAsync("1");
        Assert.Equal("John Updated", staffInDb.FullName);
    }

    [Fact]
    public async Task UpdateAsync_InvalidId_ThrowsException()
    {
        // Arrange
        var staffDto = new StaffDto
        {
            StaffId = "999",
            FullName = "John",
            Birthday = DateOnly.Parse("1990-01-01"),
            Gender = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync("999", staffDto));
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var staff = new Staff { StaffId = "1", FullName = "John", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 };
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.DeleteAsync("1");

        // Assert
        Assert.True(result);
        Assert.Null(await _context.Staffs.FindAsync("1"));
    }

    [Fact]
    public async Task DeleteAsync_InvalidId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeleteAsync("999");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ByStaffId_ReturnsFiltered()
    {
        // Arrange
        await _context.Staffs.AddRangeAsync(
            new Staff { StaffId = "1", FullName = "John", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 },
            new Staff { StaffId = "2", FullName = "Jane", Birthday = DateOnly.Parse("1995-01-01"), Gender = 2 }
        );
        await _context.SaveChangesAsync();

        var criteria = new StaffSearchCriteriaDto { StaffId = "1" };

        // Act
        var results = await _service.SearchAsync(criteria);

        // Assert
        Assert.Single(results);
        Assert.Equal("1", results.First().StaffId);
    }

    [Fact]
    public async Task SearchAsync_ByName_ReturnsFiltered()
    {
        // Arrange
        await _context.Staffs.AddRangeAsync(
            new Staff { StaffId = "1", FullName = "John Doe", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 },
            new Staff { StaffId = "2", FullName = "Jane Smith", Birthday = DateOnly.Parse("1995-01-01"), Gender = 2 }
        );
        await _context.SaveChangesAsync();

        var criteria = new StaffSearchCriteriaDto { Name = "John" };

        // Act
        var results = await _service.SearchAsync(criteria);

        // Assert
        Assert.Single(results);
        Assert.Equal("1", results.First().StaffId);
    }

    [Fact]
    public async Task SearchAsync_ByGenderAndDateRange_ReturnsFiltered()
    {
        // Arrange
        await _context.Staffs.AddRangeAsync(
            new Staff { StaffId = "1", FullName = "John", Birthday = DateOnly.Parse("1990-01-01"), Gender = 1 },
            new Staff { StaffId = "2", FullName = "Jane", Birthday = DateOnly.Parse("1995-01-01"), Gender = 2 }
        );
        await _context.SaveChangesAsync();

        var criteria = new StaffSearchCriteriaDto
        {
            Gender = 1,
            FromDate = DateOnly.Parse("1989-01-01"),
            ToDate = DateOnly.Parse("1991-01-01")
        };

        // Act
        var results = await _service.SearchAsync(criteria);

        // Assert
        Assert.Single(results);
        Assert.Equal("1", results.First().StaffId);
    }

    [Fact]
    public async Task ExportToExcelAsync_ReturnsValidExcel()
    {
        // Arrange
        await _context.Staffs.AddAsync(new Staff
        {
            StaffId = "1",
            FullName = "John",
            Birthday = DateOnly.Parse("1990-01-01"),
            Gender = 1
        });
        await _context.SaveChangesAsync();

        var criteria = new StaffSearchCriteriaDto();

        // Act
        var result = await _service.ExportToExcelAsync(criteria);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task ExportToPdfAsync_ReturnsValidPdf()
    {
        // Arrange
        await _context.Staffs.AddAsync(new Staff
        {
            StaffId = "1",
            FullName = "John",
            Birthday = DateOnly.Parse("1990-01-01"),
            Gender = 1
        });
        await _context.SaveChangesAsync();

        var criteria = new StaffSearchCriteriaDto();

        // Act
        var result = await _service.ExportToPdfAsync(criteria);

        // Assert
        Assert.NotEmpty(result);
    }
}