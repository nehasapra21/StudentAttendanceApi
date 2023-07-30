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
    public class CancelClassDto
    {
        [Required] public int Id { get; set; }   
        //public DateTime? CancelDate { get; set; }
     
        //public DateTime? EndDate { get; set; }
        [Required]
        public string? Reason { get; set; }
        [Required]
        public int? CancelBy { get; set; }

    }

}
