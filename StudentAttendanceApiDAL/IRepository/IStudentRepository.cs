using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface IStudentRepository
    {
        Task<Student> SaveStudent(Student student);
        Task<Student> GetStudentById(int id);
        Task<Student> GetStudentByCenterId(int centerId);
        Task<Student> UpdateStudentActiveOrInactive(int id, int status);
        Task<Dictionary<int, int>> GetTotalStudentPresent(DateTime scanDate, int userId);
        Task<Dictionary<int, int>> GetActiveClass(DateTime scanDate, int userId);
        Task<int> GetCancelClassCount(int userId);
        Task<Dictionary<int, int>> GetTotalUpComingAndCompletedClass(DateTime scanDate, int userId);
        Task<List<Student>> GetAllStudents(int userId, int districtId = 0, int vidhanSabhaId = 0, int panchayatId = 0, int villageId = 0);
    }
}
