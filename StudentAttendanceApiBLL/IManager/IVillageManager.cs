using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IVillageManager
    {
        Task<List<Village>> GetAllVillage(int offset, int limit);
        Task<Village> SaveVillage(Village village);
        Task<Village> GetVillageByDistrictVidhanSabhaAndPanchId(int districtId, int vidhanSabhaId, int PanchayatId);
        Task<string> CheckVillageName(string name);
    }
}
