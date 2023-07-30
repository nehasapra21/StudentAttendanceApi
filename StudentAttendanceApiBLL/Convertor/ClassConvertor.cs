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
            cls.CenterId = classDto.CenterId;
            cls.ClassEnrolmentId = classDto.ClassEnrolmentId;
            cls.Name = classDto.Name;
            cls.TotalStudents = classDto.TotalStudents;
            cls.AvilableStudents = classDto.AvilableStudents;
            //cls.EndDate = classDto.EndDate;
            //cls.Reason = classDto.Reason;
            //classDto.IsCancel = classDto.IsCancel;
            //cls.CancelDate = classDto.CancelDate;
            cls.UsersId = classDto.UserId;
            return cls;

        }

        public static Class ConvertClasstoToClass(CancelClassDto classDto)
        {
            Class cls = new Class();
            cls.Id = classDto.Id;
            cls.Reason = classDto.Reason;
            cls.CancelBy = classDto.CancelBy;
            return cls;

        }

        public static Class ConvertClasstoToEndClassDto(EndClassDto classDto)
        {
            Class cls = new Class();
            cls.Id = classDto.Id;
            return cls;

        }
    }
}
