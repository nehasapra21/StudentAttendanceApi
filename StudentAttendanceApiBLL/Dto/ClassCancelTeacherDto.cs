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
    public class ClassCancelTeacherDto
    {
        public int Id { get; set; }
        [Required]
        public int CenterId { get; set; }
        [Required]
        public DateTime? StartingDate { get; set; }
        [Required]
        public DateTime? EndingDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        [Required]
        public int? UsersId { get; set; }
        public string? Reason { get; set; }

    }
    
}
