using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class HolidaysConvertor
    {
        public static Holidays ConvertHolidaysDtoToHolidays(HolidaysDto holidaysDto)
        {
            Holidays holidays = new Holidays();
            holidays.Id = holidaysDto.Id;
            holidays.Name = holidaysDto.Name;
            holidays.Description = holidaysDto.Description;
            holidays.Status = holidaysDto.Status;
            holidays.CreatedBy = holidaysDto.CreatedBy;
            holidays.CreatedOn = holidaysDto.CreatedOn;
            holidays.StartDate = holidaysDto.StartDate;
            holidays.EndDate = holidaysDto.EndDate;
            if (!string.IsNullOrEmpty(holidaysDto.ListCenterIds))
            {
                holidays.CenterIds = holidaysDto.ListCenterIds.Split(',').Select(int.Parse).ToList();
            }
            return holidays;
        }
    }
}
