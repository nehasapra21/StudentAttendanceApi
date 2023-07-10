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
    [Table("Village")]
    public class Village
    {
        [Key]
        public int Id { get; set; }
        public int VillageGuidId { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int DistrictId { get; set; }
        public int VidhanSabhaId { get; set; }
        public int PanchayatId { get; set; }
    }

}
