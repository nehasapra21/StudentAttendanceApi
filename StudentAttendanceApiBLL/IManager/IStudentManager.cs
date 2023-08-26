using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IStudentManager
    {
        Task<Student> SaveStudent(Student student);
        Task<StudentDetailDto> GetStudentById(int id);
        Task<Student> GetStudentByCenterId(int centerId);
        Task<Student> UpdateStudentActiveOrInactive(int id,int status);
        Task<StudentPresentClassDto> GetTotalStudentPresent(int userId, int type);
    }

}
