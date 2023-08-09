using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IStudentAttendanceManager
    {
        Task<int> SaveStudentAttendance(StudentAttendance studentAttendance);
        Task<int> SaveAtuomaticStudentAttendance(StudentAttendance studentAttendance);
        Task<int> SaveManualStudentAttendance(StudentAttendance studentAttendance);
        Task<List<StudentAttendanceDetailDto>> GetAllStudentWihAvgAttendance(int centerId);
        Task<List<StudentAttendanceDetailDto>> GetAllStudentAttendancStatus(int centerId, string classDate);
        Task<List<StudentAttendanceMonthDetailDto>> GetAllStudentAttendancByMonth(int centerId,int studentId, int month,int year);
        Task<List<StudentAbsentClassDto>> GetAllAbsentAttendance(int centerId);
    }

}
