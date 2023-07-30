using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAttendanceApiDAL.Tables;
using System.Text.Json.Serialization;

namespace StudentAttendanceApiBLL
{
    public class TeacherDetailDto
    {
        public int Id { get; set; }
        public string? EnrolmentRollId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Type { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public bool? Status { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Picture { get; set; }
        public string? WhatsApp { get; set; }
        public string? LastLoginTime { get; set; }
        public string? FullAddress { get; set; }
        public string? Education { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public int? CreatedBy { get; set; }
     
        public int? VidhanSabhaId { get; set; }
        public int? DistrictId { get; set; }
        public int? VillageId { get; set; }
        public int? PanchayatId { get; set; }
        public bool? AssignedTeacherStatus { get; set; }
        public bool? AssignedRegionalAdminStatus { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianNumber { get; set; }
        public string? DistrictName { get; set; }
        public string? VidhanSabhaName { get; set; }
        public string? VillageName { get; set; }
        public string? PanchayatName { get; set; }

        public Center Center{ get; set; }
        public DateTime? CenterEnrollmentDate { get; set; }
        public DateTime? AssignedDate { get; set; }
    }
    
}
