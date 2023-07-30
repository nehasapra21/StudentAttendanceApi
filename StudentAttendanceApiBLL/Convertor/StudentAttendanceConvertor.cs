using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class StudentAttendanceConvertor
    {
        public static StudentAttendance ConvertStudentAttendancetoToStudentAttendance(StudentAttendanceDto studentAttendanceDto)
        {
            StudentAttendance studentAttendance= new StudentAttendance();
            studentAttendance.Id = studentAttendanceDto.Id;
            studentAttendance.ClassId = studentAttendanceDto.ClassId;
            studentAttendance.UserId = studentAttendanceDto.UserId;
            studentAttendance.StudentId = studentAttendanceDto.StudentId;
            studentAttendance.ScanDate = studentAttendanceDto.ScanDate; 
            studentAttendance.CenterId= studentAttendanceDto.CenterId;
            return studentAttendance;

        }

        public static StudentAttendanceDetailDto ConvertStudentToStudentAttendanceDeatilDto(Student studentAttendanceDto)
        {
            StudentAttendanceDetailDto studentAttendance = new StudentAttendanceDetailDto();
            studentAttendance.Id = studentAttendanceDto.Id;
            studentAttendance.EnrollmentId = studentAttendanceDto.EnrollmentId;
            studentAttendance.FullName = studentAttendanceDto.FullName;
            studentAttendance.AttendanceStatus = studentAttendanceDto.StudentStaus;
            studentAttendance.AverageAttendance =Math.Round(studentAttendanceDto.AvgAttendance,2);
            studentAttendance.JoningDate = studentAttendanceDto.CreatedOn;
            return studentAttendance;

        }
    }
}
