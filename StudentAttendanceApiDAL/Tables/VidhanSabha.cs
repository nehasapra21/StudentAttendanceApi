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
    [Table("VidhanSabha")]
    public class VidhanSabha
    {
        [Key]
        public int Id { get; set; }
        public Guid VidhanSabhaGuidId { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int DistrictId { get; set; }
        [NotMapped]
        public string DistrictName { get; set; }
    }

}
