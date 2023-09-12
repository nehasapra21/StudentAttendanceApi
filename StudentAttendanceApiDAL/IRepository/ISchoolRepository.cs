using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.IRepository
{
    public interface ISchoolRepository
    {
        Task<School> SaveSchool(School school);
        Task<List<School>> GetAllSchools();
    }
}
