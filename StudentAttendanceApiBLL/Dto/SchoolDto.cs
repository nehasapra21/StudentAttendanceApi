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
    public class SchoolDto
    {
        public int Id { get; set; }
        [Required]
        public string? SchoolName { get; set; }
      
        public DateTime? CreatedOn { get; set; }
      
        public int? CreatedBy { get; set; }
     

    }
    
}
