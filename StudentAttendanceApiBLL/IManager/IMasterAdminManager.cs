using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IMasterAdminManager
    {
        Task<List<MasterAdmin>> GetAllMasterAdmin();
        Task<MasterAdmin> SaveMasterAdmin(MasterAdmin masterAdmin);
        Task<MasterAdmin> LoginSuperAdmin(string name, string password);
    }
}
