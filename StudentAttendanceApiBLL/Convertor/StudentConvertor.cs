using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class StudentConvertor
    {
        public static Student ConvertStudenttoToStudent(StudentDto studentDto)
        {
            Student student= new Student();
            student.Id = studentDto.Id;
            student.EnrollmentId = studentDto.EnrollmentId;
            student.FullName = studentDto.FullName;
            student.FullAddress = studentDto.FullAddress;
            student.Status = studentDto.Status;
            student.Age = studentDto.Age;
            student.Gender = studentDto.Gender;
            student.DateOfBirth = studentDto.DateOfBirth;
            student.PhoneNumber = studentDto.PhoneNumber;
            student.WhatsApp = studentDto.WhatsApp;
            student.Email = studentDto.Email;
            student.Contact = studentDto.Contact;
            student.Counter = studentDto.Counter;
            student.Grade = studentDto.Grade;
            student.Remarks = studentDto.Remarks;
            student.ProfileImage = studentDto.ProfileImage;
            student.LastClass = studentDto.LastClass;
            student.CreatedBy = studentDto.CreatedBy;
            student.CreatedOn = studentDto.CreatedOn;
            student.DistrictId = studentDto.DistrictId;
            student.VidhanSabhaId = studentDto.VidhanSabhaId;
            student.VillageId = studentDto.VillageId;
            student.PanchayatId = studentDto.PanchayatId;
            student.CenterId = studentDto.CenterId;
            student.FatherName = studentDto.FatherName;
            student.MotherName = studentDto.MotherName;
            return student;

        }
    }
}
