using Microsoft.Extensions.Configuration;
using StudentAttendanceApiBLL.IManager;
using StudentAttendanceApiBLL.Manager;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//For JW Token

var appsettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appsettingSection);

var appsetting = appsettingSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appsetting.Key);

builder.Services.AddAuthentication(au =>
{
    au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

    };
});

builder.Services.AddTransient<IMasterAdminRepository, MasterAdminRepository>();
builder.Services.AddTransient<IMasterAdminManager, MasterAdminManager>();
builder.Services.AddTransient<IVidhanSabhaRepository, VidhanSabhaRepository>();
builder.Services.AddTransient<IVidhanSabhaManager, VidhanSabhaManager>();
builder.Services.AddTransient<IVillageRepository, VillageRepository>();
builder.Services.AddTransient<IVillageManager, VillageManager>();
builder.Services.AddTransient<IDistrictRepository, DistrictRepository>(); builder.Services.AddTransient<IDistrictManager, DistrictManager>();

builder.Services.AddTransient<IPanchayatRepository, PanchayatRepository>();
builder.Services.AddTransient<IPanchayatManager, PanchayatManager>();

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
