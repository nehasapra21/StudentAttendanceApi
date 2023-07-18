using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace StudentAttendanceApiBLL
{
    public class RegionalAdminDto
    { 
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public Guid RegionalAdminGuidId { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
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
        [Required]
        public int VidhanSabhaId { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int PanchayatId { get; set; }
        [Required]
        public int VillageId { get; set; }
        [Required]
        public int CenterId { get; set; }
    }

}
