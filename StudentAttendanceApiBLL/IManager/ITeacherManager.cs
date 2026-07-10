using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface ITeacherManager
    {
        Task<Teacher> LoginTeacher(string name, string password);
        Task<Teacher> SaveTeacher(Teacher teacher);
    }

}
