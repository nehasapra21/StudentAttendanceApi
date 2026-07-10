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
        Task<List<AllCenterDto>> GetAllCenters(int userId,int type);
        Task<CenterDetailDto> GetCenteryId(int centerId);
        Task<List<AllCenterStatusDto>> GetStudentAttendanceOfCenter(int status, int userId);
        Task<CenterDetailDto> GetCenterByUserId(int userId);
        Task<CenterLog> UpdateCenterActiveOrDeactive(CenterLogDto centerLogDto);
        Task<List<CenterAttendanceDto>> GetAllCenterAttendance(int userId,string date,int offset, int limit);
        Task<string> GetTotalAttendanceCountOfCenter(int userId, string date);
    }

}
