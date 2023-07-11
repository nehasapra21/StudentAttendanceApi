using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IRegionalAdminManager
    {
        Task<List<RegionalAdmin>> GetAllRegionalAdmin();
        Task<RegionalAdmin> SaveRegionalAdmin(RegionalAdmin masterAdmin);
    }
}
