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
    public class SuperAdminDetailDto
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
     
        public string? GuardianName { get; set; }
        public string? GuardianNumber { get; set; }
       
    }
    
}
