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
        Task<Dictionary<int, int>> GetTotalStudentPresent();
 
    }
}
