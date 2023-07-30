using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public class StudentAttendanceDto
    {
        public int Id { get; set; }
        [Required]
        public int? ClassId { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int? StudentId { get; set; }
        [Required]
        public DateTime? ScanDate { get; set; }
        public int? CenterId { get; set; }

    }

}
