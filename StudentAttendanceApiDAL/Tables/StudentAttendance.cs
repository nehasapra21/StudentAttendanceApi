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
    [Table("StudentAttendance")]
    public class StudentAttendance
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? CenterId { get; set; }
        public int? UserId { get; set; }
        public bool? ManualAttendance { get; set; }
        public int? StudentId { get; set; }
        public DateTime? ScanDate { get; set; }


    }
    
}
