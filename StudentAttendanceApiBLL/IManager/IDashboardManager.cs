using Newtonsoft.Json.Linq;
using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IDashboardManager
    {
        Task<string> GetClassCountByMonth(int centerId, int month);
        Task<string> GetTotalGenderRatioByCenterId(int centerId);
        Task<string> GetTotalStudentOfClass(int centerId);
    }
}
