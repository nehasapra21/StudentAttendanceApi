using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public class StudentDetailDto
    {
        public int Id { get; set; }
        public string? EnrollmentId { get; set; }
        public string? FullName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public string? DateOfBirth { get; set; }

        public string? Email { get; set; }
        public string? Remarks { get; set; }
        public string? Grade { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImage { get; set; }
        public string? WhatsApp { get; set; }
        public string? FullAddress { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string CenterName { get; set; }
        public string TeacherName { get; set; }
        public int CenterId { get; set; }
        public int DistrictId { get; set; }
        public int VidhanSabhaId { get; set; }
        public int? VillageId { get; set; }
        public int PanchayatId { get; set; }
    }

}
