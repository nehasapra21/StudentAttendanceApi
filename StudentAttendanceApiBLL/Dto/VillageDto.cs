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
    public class VillageDto
    {
        public int Id { get; set; }
        public Guid VillageGuidId { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool? Status { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int VidhanSabhaId { get; set; }
        [Required]
        public int PanchayatId { get; set; }
    }

}
