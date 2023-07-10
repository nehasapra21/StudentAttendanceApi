using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IMasterAdminRepository
    {
       public Task<List<MasterAdmin>> GetAllMasterAdmin();
        Task<MasterAdmin> SaveMasterAdmin(MasterAdmin masterAdmin);
        Task<MasterAdmin> LoginSuperAdmin(string name, string password);
    }
}
