using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IDashboardRepository
    {
        Task<dynamic> GetClassCountByMonth(int centerId, int month);
        Task<dynamic> GetTotalGenderRatioByCenterId(int centerId);
        Task<dynamic> GetTotalStudentOfClass(int centerId);
    }
}
