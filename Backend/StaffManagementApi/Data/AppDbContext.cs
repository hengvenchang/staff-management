using Microsoft.EntityFrameworkCore;
using StaffManagementApi.Models;

namespace StaffManagementApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Staff> Staffs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=staff.db");
        }
    }
}