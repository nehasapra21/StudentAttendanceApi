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
        Task<Center> GetCenteryId(int centerId);
        Task<List<Center>> GetAllCenters();
        Task<List<Center>> GetStudentAttendanceOfCenter(int status);
        Task<Center> GetCenterByUserId(int userId);
        Task<List<Center>> GetAllCentersById(int districtId, int vidhanSabhaId, int panchayatId, int villageId);
    }
}
