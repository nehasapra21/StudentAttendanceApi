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
    public class UserDto
    {
        public int Id { get; set; }
        public string? EnrolmentRollId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Token { get; set; }
        
        public string? Email { get; set; }
        [Required]
        public int? Type { get; set; }
        public int? Age { get; set; }
        [Required]
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public bool? Status { get; set; }
        [Required]
        public string? DateOfBirth { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? Picture { get; set; }
        [Required]
        public string? WhatsApp { get; set; }
        public string? LastLoginTime { get; set; }
        public string? FullAddress { get; set; }
        public string? Education { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        [Required]
        public int? CreatedBy { get; set; }
     
        public int? VidhanSabhaId { get; set; }
        public int? DistrictId { get; set; }
        public int? VillageId { get; set; }
        public bool? AssignedTeacherStatus { get; set; }
        public bool? AssignedRegionalAdminStatus { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianNumber { get; set; }
        public List<int>? ListOfPanchayatId { get; set; }
    }
    
}
