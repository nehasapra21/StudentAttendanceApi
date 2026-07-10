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
    public class StudentAbsentClassDto
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string EnrollmentId { get; set; }
        public string ProfileImage { get; set; }
        public int? ManualAttendance { get; set; }

    }
    
}
