using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface ICenterManager
    {
        Task<Center> SaveCenter(Center center);
        Task<string> CheckCenterName(string name);
        Task<List<Center>> GetAllCenters();
        Task<CenterDetailDto> GetCenteryId(int centerId); 
        Task<List<Center>> GetStudentAttendanceOfCenter(int status);
        Task<List<Center>> GetAllCentersById(int districtId, int vidhanSabhaId, int panchayatId, int villageId);
    }

}
