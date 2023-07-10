using Microsoft.Extensions.Configuration;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<IMasterAdminRepository, MasterAdminRepository>();
builder.Services.AddTransient<IVidhanSabhaRepository, VidhanSabhaRepository>();
builder.Services.AddTransient<IVidhanSabhaManager, VidhanSabhaManager>();
builder.Services.AddTransient<IVillageRepository, VillageRepository>();
//builder.Services.AddTransient<IVillageManager, VillageManager>();
builder.Services.AddTransient<IDistrictRepository, DistrictRepository>();
//builder.Services.AddTransient<IDistrictManager, DistrictManager>();

builder.Services.AddTransient<IPanchayatRepository, PanchayatRepository>();
//builder.Services.AddTransient<IPanchayatManager, PanchayatManager>();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbDatabase")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    //  c.RoutePrefix = "v1";
    // var basePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    // c.SwaggerEndpoint($"{basePath}/swagger/{c.RoutePrefix}/swagger.json", "Role");
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "role");

    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "role");
    c.RoutePrefix = "swagger";
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
