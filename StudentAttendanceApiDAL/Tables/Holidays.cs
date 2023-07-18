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
    [Table("Holidays")]
    public class Holidays
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? SelectedDate { get; set; }
        public int? CreatedBy{ get; set; }
        public int CenterId { get; set; }
        [NotMapped]
        public List<int> CenterIds { get; set; }
        [NotMapped]
        public string? CenterName { get; set; }

    }
    
}
