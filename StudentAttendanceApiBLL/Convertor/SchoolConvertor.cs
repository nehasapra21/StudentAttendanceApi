using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class SchoolConvertor
    {
        public static School ConvertSchoolDtoToSchool(SchoolDto schoolDto)
        {
            School school = new School();
            school.Id = schoolDto.Id;
            school.SchoolName = schoolDto.SchoolName;
            school.CreatedBy = schoolDto.CreatedBy;
            school.CreatedOn = schoolDto.CreatedOn;
        
            return school;

        }


     
    }
}
