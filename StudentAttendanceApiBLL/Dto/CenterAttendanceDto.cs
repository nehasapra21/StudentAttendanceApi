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
    public class CenterAttendanceDto
    {
        public int Id { get; set; }
        public string? CenterName { get; set; }
      
      
        public DateTime? StartedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalStudents { get; set; }
        public int? PresentStudents { get; set; }

        public string? RegionalAdminName { get; set; }
        public string? TeacherName { get; set; }



    }
    
}
