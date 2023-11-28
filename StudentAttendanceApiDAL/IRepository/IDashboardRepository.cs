using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IDashboardRepository
    {
        Task<string> GetClassCountByMonth(int centerId, int month);
        Task<string> GetTotalGenderRatioByCenterId(int centerId);
        Task<string> GetTotalStudentOfClass(int centerId);
        Task<string> GetCenterDetailByMonth(int centerId, int month, int year);
        Task<string> GetTotalBpl(int centerId, bool BplValue);
        Task<string> GetTotalStudentCategoryOfClass(int centerId);
    }
}
