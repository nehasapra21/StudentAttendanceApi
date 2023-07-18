using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class TeacherConvertor
    {
        public static Teacher ConvertTeachertoToTeacher(TeacherDto teacherDto)
        {
            Teacher teacher = new Teacher();
            teacher.Id = teacherDto.Id;
            teacher.TeacherGuidId = teacherDto.TeacherGuidId;
            teacher.FullName = teacherDto.FullName;
            teacher.Token = teacherDto.Token;
            teacher.FullAddress = teacherDto.FullAddress;
            teacher.Status = teacherDto.Status;
            teacher.Age = teacherDto.Age;
            teacher.Gender = teacherDto.Gender;
            teacher.DateOfBirth = teacherDto.DateOfBirth;
            teacher.PhoneNumber = teacherDto.PhoneNumber;
            teacher.WhatsApp = teacherDto.WhatsApp;
            teacher.Email = teacherDto.Email;
            teacher.Count = teacherDto.Count;
            teacher.Picture = teacherDto.Picture;
            teacher.Education = teacherDto.Education;
            teacher.LastLoginTime = teacherDto.LastLoginTime;
            teacher.Password = teacherDto.Password;
            teacher.CreatedBy = teacherDto.CreatedBy;
            teacher.CreatedOn = teacherDto.CreatedOn;
            teacher.DistrictId = teacherDto.DistrictId;
            teacher.VidhanSabhaId = teacherDto.VidhanSabhaId;
            teacher.VillageId = teacherDto.VillageId;
            teacher.PanchayatId = teacherDto.PanchayatId;
            teacher.CenterId = teacherDto.CenterId;
            return teacher;

        }
    }
}
