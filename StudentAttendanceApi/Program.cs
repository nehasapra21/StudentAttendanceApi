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
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using StudentAttendanceApi;
using StudentAttendanceApi.FCM;
using CorePush.Apple;
using CorePush.Google;
using StudentAttendanceApi.Services;
using System.IO;
using StudentAttendanceApi.ActivityLog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Org.BouncyCastle.Ocsp;

var builder = WebApplication.CreateBuilder(args);

//For JW Token

var appsettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appsettingSection);

var appsetting = appsettingSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appsetting.Key);

//builder.Services.AddDefaultIdentity<IdentityUser>(... )
//    .AddRoles<IdentityRole>();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy(AllDevops.ToString(), policy => policy.RequireRole(RoleDevOpsName));
//    options.AddPolicy(AdminAndDevops.ToString(), policy => policy.RequireRole(RoleAdminName,
//                                                                   RoleDevOpsName));
//    options.AddPolicy(AdminSupportConsultant.ToString(), policy => policy.RequireRole(RoleAdminName,
//                                                                    RoleSupportName,
//                                                                    RoleConsultantName));
//    options.AddPolicy(AdminSupportConsultantDevops.ToString(), policy => policy.RequireRole(RoleAdminName,
//                                                                   RoleSupportName,
//                                                                   RoleConsultantName,
//                                                                   RoleDevOpsName));
//    options.AddPolicy(All.ToString(), policy => policy.RequireRole(RoleDevOpsName, RoleAdminName,
//                   RoleSupportName, RoleConsultantName, RoleViewerName));
//});

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
        RequireExpirationTime=true, 
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //helo
    };
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.LoginPath = new PathString("/User");
             });

///register services
builder.Services.AddTransient<IStudentAttendanceManager, StudentAttendanceManager>();
builder.Services.AddTransient<IStudentAttendanceRepository, StudentAttendanceRepository>();
builder.Services.AddTransient<IClassRepository, ClassRepository>();
builder.Services.AddTransient<IClassManager, ClassManager>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<ICenterRepository, CenterRepository>();
builder.Services.AddTransient<ICenterManager, CenterManager>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();
builder.Services.AddTransient<ITeacherManager, TeacherManager>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<IStudentManager, StudentManager>();
builder.Services.AddTransient<IRegionalAdminRepository, RegionalAdminRepository>();
builder.Services.AddTransient<IRegionalAdminManager, RegionalAdminManager>();
builder.Services.AddTransient<IVidhanSabhaRepository, VidhanSabhaRepository>();
builder.Services.AddTransient<IVidhanSabhaManager, VidhanSabhaManager>();
builder.Services.AddTransient<IVillageRepository, VillageRepository>();
builder.Services.AddTransient<IVillageManager, VillageManager>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IDashboardManager, DashboardManager>();
builder.Services.AddTransient<IDistrictRepository, DistrictRepository>(); builder.Services.AddTransient<IDistrictManager, DistrictManager>();

builder.Services.AddTransient<IAnnouncementRepository, AnnouncementRepository>(); builder.Services.AddTransient<IAnnouncementManager, AnnouncementManager>();

builder.Services.AddTransient<IPanchayatRepository, PanchayatRepository>();
builder.Services.AddTransient<IPanchayatManager, PanchayatManager>();

builder.Services.AddTransient<IHolidaysRepository, HolidaysRepository>();
builder.Services.AddTransient<IHolidaysManager, HolidaysManager>();

builder.Services.AddTransient<ISchoolRepository, SchoolRepository>();
builder.Services.AddTransient<ISchoolManager, SchoolManager>();

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers();

//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add(typeof(UserActivityFilter));
//});

builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbDatabase")));

builder.Services.Configure<ConnectionString>(myOptions =>
{
    myOptions.ConnString = builder.Configuration.GetConnectionString("DbDatabase");
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentAttendance Api", Version = "v1.0" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                       {
                            new OpenApiSecurityScheme
                                   {
                                    Reference = new OpenApiReference
                                      {
                                           Type=ReferenceType.SecurityScheme,
                                            Id="Bearer"
                                     }
                         },
                             new string[]{}
                            }
                       });
});

//FCM notification\
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddHttpClient<FcmSender>();
builder.Services.AddHttpClient<ApnSender>();

// Configure strongly typed settings objects
var appSettingsSection = builder.Configuration.GetSection("FcmNotification");
builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);

//logging
 var path = Directory.GetCurrentDirectory();

//builder.Services.AddLogging(builder =>
//{
//    var logFilePath = $"{path}\\Logs\\api\\Log.txt"; // Customize the path as needed
//                                                     //  var logFilePath = hostContext.Configuration["Serilog:FilePath"];

//    Log.Logger = new LoggerConfiguration()
//        .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
//        .CreateLogger();

//    builder.AddSerilog();
//});
//\

//rest identity coloumn of table
//DBCC CHECKIDENT('[tablename]', RESEED, 0);
//GO


//
//delete from school
//delete from student
//delete studentattendance
//delete from center
//delete from useractivitylog
//delete from users where id not in (1)
//delete from concern
//delete from CenterAssignUser
//delete from ClassCancelByTeacher
//delete from TeacherActivityLog
//delete from Holidays
//delete from Class
//delete from ClassDetail

//delete from RegionalAdminPanchayat
//delete from RegionalAdmin
//delete from center


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
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
