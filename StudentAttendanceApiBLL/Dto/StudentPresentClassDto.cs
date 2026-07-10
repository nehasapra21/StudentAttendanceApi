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
    public class StudentPresentClassDto
    {
        public int? TotalStudents { get; set; }
        public int? PresentStudents { get; set; }

        public int? TotalClasses { get; set; }
        public int? TotalActiveClasses { get; set; }

        public int? CompletedClassCount { get; set; }
        public int? UpComingClassCount { get; set; }
        public int? CancelClassCount { get; set; }
        public TimeZoneInfo time { get; set; }

    }
    
}
