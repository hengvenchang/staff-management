using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using StaffManagementApi.Data;
using StaffManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Enable CORS for frontend (update origins as needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Data context and SQLite setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=staff.db"));
    
builder.Services.AddAutoMapper(typeof(Program));  // For later DTOs

// Add StaffService
builder.Services.AddScoped<IStaffService, StaffService>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// In app.Use... section:
app.UseCors("AllowAll");

app.Run();