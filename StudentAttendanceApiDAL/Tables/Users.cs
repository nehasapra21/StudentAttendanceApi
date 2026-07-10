using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiDAL.Tables
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string? EnrolmentRollId { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public string? DeviceId { get; set; }
        public int? Type { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public bool? Status { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Picture { get; set; }
        public string? WhatsApp { get; set; }
        public string? LastLoginTime { get; set; }
        public string? FullAddress { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianNumber { get; set; }
        public string? Education { get; set; }
        public int? CreatedBy { get; set; }
        public int? VidhanSabhaId { get; set; }
        public int? DistrictId { get; set; }
        //public int? PanchayatId { get; set; }
        public int? VillageId { get; set; }
        public int? PanchayatId { get; set; }
        public bool? AssignedTeacherStatus { get; set; }
        public bool? AssignedRegionalAdminStatus { get; set; }
        [NotMapped]
        public List<int> ListOfPanchayatId { get; set; }
        public District District { get; set; }
        public VidhanSabha VidhanSabha { get; set; }
        public Panchayat Panchayat { get; set; }
        public Village Village { get; set; }
        public ICollection<RegionalAdminPanchayat> RegionalAdminPanchayat { get; set; }
        [NotMapped]
        public Center Center { get; set; }
        [NotMapped]
        public List<Center> Centers { get; set; }
        public CenterAssignUser CenterAssignUser { get; set; }
    }

}
