using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceApiDAL.Tables
{
    [Table("RegionalAdmin")]
    public class RegionalAdmin
    {
        [Key]
        public int Id { get; set; }
        public Guid RegionalAdminGuidId { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Contact { get; set; }
        public bool? Status { get; set; }
        public int? Type { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Picture { get; set; }
        public string? WhatsApp { get; set; }
        public string? LastLoginTime { get; set; }
        public string? FullAddress { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int VidhanSabhaId { get; set; }

        public int DistrictId { get; set; }

        public int PanchayatId { get; set; }

        public int VillageId { get; set; }

        public int CenterId { get; set; }

    }
}
