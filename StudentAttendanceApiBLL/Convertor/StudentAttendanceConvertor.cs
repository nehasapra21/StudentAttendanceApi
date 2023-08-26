using Microsoft.Extensions.Logging;
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
            StudentAttendance studentAttendance = new StudentAttendance();
            studentAttendance.Id = studentAttendanceDto.Id;
            studentAttendance.ClassId = studentAttendanceDto.ClassId;
            studentAttendance.UserId = studentAttendanceDto.UserId;

            
            if (!string.IsNullOrEmpty(studentAttendanceDto.StudentIds))
            {
                studentAttendance.ListOfStudentIds = studentAttendanceDto.StudentIds.Split(',').Select(int.Parse).ToList();
            }
            else
            {
                studentAttendance.StudentId = Convert.ToInt32(studentAttendanceDto.StudentIds);

            }

            studentAttendance.ScanDate = studentAttendanceDto.ScanDate;
            studentAttendance.CenterId = studentAttendanceDto.CenterId;
            return studentAttendance;

        }

        public static StudentAttendanceDetailDto ConvertStudentToStudentAttendanceDeatilDto(Student studentAttendanceDto)
        {
            StudentAttendanceDetailDto studentAttendance = new StudentAttendanceDetailDto();
            studentAttendance.Id = studentAttendanceDto.Id;
            studentAttendance.EnrollmentId = studentAttendanceDto.EnrollmentId;
            studentAttendance.FullName = studentAttendanceDto.FullName;
            studentAttendance.AttendanceStatus = studentAttendanceDto.StudentStaus;
            studentAttendance.AverageAttendance = Math.Round(studentAttendanceDto.AvgAttendance, 2);
            studentAttendance.Date = studentAttendanceDto.JoiningDate;
            return studentAttendance;

        }

        public static StudentAttendanceMonthDetailDto ConvertStudentToStudentAttendanceMonthDeatilDto(Student studentAttendanceDto)
        {
            StudentAttendanceMonthDetailDto studentAttendance = new StudentAttendanceMonthDetailDto();
            studentAttendance.Id = studentAttendanceDto.Id;
            studentAttendance.FullName = studentAttendanceDto.FullName;
            studentAttendance.AttendanceStatus = studentAttendanceDto.StudentStaus;
            studentAttendance.Date = studentAttendanceDto.CreatedOn;
            return studentAttendance;

        }

        public static StudentAbsentClassDto ConvertStudentToStudentAbsentClassDtoDto(Student studentAttendanceDto)
        {
            StudentAbsentClassDto studentAttendance = new StudentAbsentClassDto();
            studentAttendance.Id = studentAttendanceDto.Id;
            studentAttendance.EnrollmentId = studentAttendanceDto.EnrollmentId;
            studentAttendance.Name = studentAttendanceDto.FullName;
            studentAttendance.ProfileImage = studentAttendanceDto.ProfileImage;
            studentAttendance.ManualAttendance = studentAttendanceDto.ManualAttendance;
            return studentAttendance;

        }
    }
}
