using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Users> Users { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<VidhanSabha> VidhanSabha { get; set; }
        public DbSet<Panchayat> Panchayat { get; set; }
        public DbSet<Village> Village { get; set; }
    }
}
