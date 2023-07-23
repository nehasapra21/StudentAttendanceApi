using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL.IManager
{
    public interface IHolidaysManager
    {
        Task<Holidays> SaveHolidays(Holidays holidays);
        Task<List<Holidays>> GetAllHolidaysByTeacherId(int teacherId, string selecteddate);
        Task<List<Holidays>> GetAllHolidaysByYear(int year);
        Task<List<Holidays>> GetAllHolidaysByCenterId(int centerId);


    }

}
