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
    public class TeacherDto
    {
        public int Id { get; set; }
        public Guid TeacherGuidId { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? FullName { get; set; }
        public string? Token { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public bool? Status { get; set; }
        public int? Count { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Picture { get; set; }
        public string? WhatsApp { get; set; }
        public string? LastLoginTime { get; set; }
        public string? FullAddress { get; set; }
        public DateTime? CreatedOn { get; set; }
        [Required]
        public int VidhanSabhaId { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int PanchayatId { get; set; }
        [Required]
        public int CenterId { get; set; }
        public int? CreatedBy { get; set; }
        [Required]
        public int VillageId { get; set; }
        public string? Education { get; set; }



    }

}
