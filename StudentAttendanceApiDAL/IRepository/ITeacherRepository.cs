using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface ITeacherRepository
    {
        Task<Teacher> LoginTeacher(string name, string password);
        Task<Teacher> SaveTeacher(Teacher teacher);
    }
}
