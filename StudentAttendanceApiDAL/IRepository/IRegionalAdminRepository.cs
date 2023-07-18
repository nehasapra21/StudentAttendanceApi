using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IRegionalAdminRepository
    {
        Task<RegionalAdmin> LoginRegionalAdmin(string name, string password);
        Task<RegionalAdmin> SaveRegionalAdmin(RegionalAdmin masterAdmin);
        public Task<List<RegionalAdmin>> GetAllRegionalAdmin();
    }
}
