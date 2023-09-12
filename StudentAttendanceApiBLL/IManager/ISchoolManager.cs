using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface ISchoolManager
    {
        Task<School> SaveSchool(School school);
        Task<List<School>> GetAllSchools();
    }

}
