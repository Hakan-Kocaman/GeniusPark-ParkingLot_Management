
using Microsoft.EntityFrameworkCore;
using ParkingLot.API.Services;
using ParkingLot.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<ParkingLotDbContext>(options =>
    options.UseSqlServer("Server=HAKAN\\SQLEXPRESS01;Database=PARKINGLOTDB;Trusted_Connection=True;Encrypt=False;"));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PreloadService>();
builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<DetectPlateService>();


builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();