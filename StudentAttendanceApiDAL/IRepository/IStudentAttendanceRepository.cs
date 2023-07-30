using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IStudentAttendanceRepository
    {
        Task<int> SaveStudentAttendance(StudentAttendance studentAttendance);
        Task<List<Student>> GetAllStudentWihAvgAttendance(int centerId);
        Task<List<Student>> GetAllStudentAttendancStatus(int centerId, string classDate);
        Task<List<Student>> GetAllStudentAttendancByMonth(int centerId,int studentId, int month);
        

    }
}
