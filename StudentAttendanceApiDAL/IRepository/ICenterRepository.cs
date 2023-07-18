using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface ICenterRepository
    {
        Task<Center> SaveCenter(Center center);
        Task<string> CheckCenterName(string name);
        Task<List<Center>> GetAllCenters();
        Task<List<Center>> GetAllCentersById(int districtId, int vidhanSabhaId, int panchayatId, int villageId);
    }
}
