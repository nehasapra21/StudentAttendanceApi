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
    [Table("School")]
    public class School
    {
        public int Id { get; set; }
        public string? SchoolName { get; set; }
      
        public DateTime? CreatedOn { get; set; }
      
        public int? CreatedBy { get; set; }

    }
    
}
