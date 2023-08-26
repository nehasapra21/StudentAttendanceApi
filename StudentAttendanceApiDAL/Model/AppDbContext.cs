using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceApi.ActivityLog;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.Model
{
    public class AppDbContext : DbContext, IDataProtectionKeyContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }

        // This maps to the table that stores keys.
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public DbSet<RegionalAdmin> RegionalAdmin { get; set; }
        public DbSet<RegionalAdminPanchayat> RegionalAdminPanchayat { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<CenterAssignUser> CenterAssignUser { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Center> Center { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentAttendance> StudentAttendance { get; set; }
        public DbSet<Holidays> Holidays { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<VidhanSabha> VidhanSabha { get; set; }
        public DbSet<Panchayat> Panchayat { get; set; }
        public DbSet<Village> Village { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<ClassCancelTeacher> ClassCancelTeacher { get; set; }
        public DbSet<UserActivityLog> UserActivityLog { get; set; }
    }
}
