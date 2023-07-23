using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class ClassConvertor
    {
        public static Class ConvertClasstoToClass(ClassDto classDto)
        {
            Class cls = new Class();
            cls.Id = classDto.Id;
            cls.ClassEnrolmentId = classDto.ClassEnrolmentId;
            cls.Name = classDto.Name;
            cls.Status = classDto.Status;
            cls.TotalStudents = classDto.TotalStudents;
            cls.AvilableStudents = classDto.AvilableStudents;
            cls.StartedDate = classDto.StartedDate;
            cls.EndDate = classDto.EndDate;
            cls.Reason = classDto.Reason;
            classDto.CancelBy = classDto.CancelBy;
            return cls;

        }
    }
}
