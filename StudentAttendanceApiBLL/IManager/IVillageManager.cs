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
        Task<List<Village>> GetAllVillage();
        Task<Village> SaveVillage(Village village);
        Task<Village> GetVillageByDistrictVidhanSabhaAndPanchId(int districtId, int vidhanSabhaId, int PanchayatId);
    }
}
