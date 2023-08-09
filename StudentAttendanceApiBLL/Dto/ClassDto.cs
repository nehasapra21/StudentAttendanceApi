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
    public class ClassDto
    {
        public int Id { get; set; }
        public string? ClassEnrolmentId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int CenterId { get; set; }
       
        //public DateTime? CancelDate { get; set; }
        
        //public DateTime? EndDate { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int TotalStudents { get; set; }
        [Required]
        public int? AvilableStudents { get; set; }
        //public string? Reason { get; set; }
        //public bool? IsCancel { get; set; }

    }

}
