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
    [Table("ClassCancelByTeacher")]
    public class ClassCancelTeacher
    {
        [Key]
        public int Id { get; set; }
        public int CenterId { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UserId { get; set; }
        public string? Reason { get; set; }
        [NotMapped]
        public Center center { get; set; }
    }
    
}
